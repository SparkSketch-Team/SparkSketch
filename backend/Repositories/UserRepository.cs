using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Azure.Core;
using backend.Services;

public class UserRepository : BaseRepository, IUserRepository
{
    private readonly IAccountEmailSender _emailSender;

    public UserRepository(IConfiguration config, IAccountEmailSender emailSender) : base(config)
    {
        _emailSender = emailSender;
    }
    public async Task<Claim[]> GetUserClaims(LoginInfo loginInfo)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Username == loginInfo.username && u.IsActive);
        List<Claim> claims = new List<Claim>();
        if (user != null)
        {
            var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.UserId);
            //var userPermission = await db.Permissions.FirstOrDefaultAsync(p => p.PermissionId == user.UserPermission.PermissionId);
            //Console.WriteLine("Check");

            claims.Add(new Claim(SparkSketchClaims.UserId, user.UserId.ToString()));
            if (userRole != null)
            {
                claims.Add(new Claim(SparkSketchClaims.Role, userRole.RoleId.ToString()));
            }
            if (user.UserPermission != null)
            {
                claims.Add(new Claim(SparkSketchClaims.PermissionLevel, user.UserPermission.PermissionLevel.ToString()));
            }

            //var userPermission = await db.Permissions.Where(p => p.PermissionId == user.UserPermission.PermissionId).ToListAsync();

            //claims.Add(new Claim(CoordinatwosClaims.PermissionLevel, userPermission[0].PermissionLevel.ToString()));

        }
        return claims.ToArray();
    }

    public async Task<UserDTSummary> GetUsersDT(UserDTRequest userDTRequest)
    {
        DTRequest dtRequest = userDTRequest.dTRequest;
        // var usersQuery = db.Users.Include(x => x).AsQueryable();
        var usersQuery = db.Users.Include(u => u.UserPermission).AsQueryable();
        var recordsTotal = await usersQuery.CountAsync();

        if (dtRequest.search != null && dtRequest.search.Length > 0)
        {
            var searchTerms = dtRequest.search.ToLower().Split(' ');
            foreach (var searchTerm in searchTerms)
            {
                usersQuery = usersQuery.Where(x => x.FirstName.ToLower().Contains(searchTerm) ||
                x.LastName.ToLower().Contains(searchTerm));
            }
        }

        // if (dTRequest.isActive != null) {

        // }

        // if (dTRequest.role != null) {

        // }

        switch (dtRequest.orderColumn)
        {
            case 0:
                if (dtRequest.orderIsAscending)
                {
                    usersQuery = usersQuery.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
                }
                else
                {
                    usersQuery = usersQuery.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName);
                }
                break;
            case 1:
                if (dtRequest.orderIsAscending)
                {
                    usersQuery = usersQuery.OrderBy(x => x.EmailAddress);
                }
                else
                {
                    usersQuery = usersQuery.OrderByDescending(x => x.EmailAddress);
                }
                break;
            case 2:
                if (dtRequest.orderIsAscending)
                {
                    usersQuery = usersQuery.OrderBy(x => x.UserPermission.PermissionLevel);
                }
                else
                {
                    usersQuery = usersQuery.OrderByDescending(x => x.UserPermission.PermissionLevel);
                }
                break;
            case 3:
                if (dtRequest.orderIsAscending)
                {
                    usersQuery = usersQuery.OrderBy(x => x.IsActive);
                }
                else
                {
                    usersQuery = usersQuery.OrderByDescending(x => x.IsActive);
                }
                break;
        }
        var recordsFiltered = await usersQuery.CountAsync();
        //Console.WriteLine("Getting Users Summaries");
        var users = await usersQuery.Skip(dtRequest.start).Take(dtRequest.length).Select(x => UserSummary.Assemble(x)).ToListAsync();

        return new UserDTSummary()
        {
            data = users,
            recordsFiltered = recordsFiltered,
            recordsTotal = recordsTotal,
            draw = dtRequest.draw
        };

    }

    public async Task<UserSummary?> GetUser(Guid userId)
    {
        var user = await db.Users.Include(x => x.UserPermission).Where(x => x.UserId == userId).FirstOrDefaultAsync();
        if (user != null)
        {
            return UserSummary.Assemble(user);
        }
        return null;
    }

    public async Task<UserSummary?> GetUserAi(String firstName)
    {
        var myUser = await db.Users.Include(x => x.UserPermission).Where(x => x.FirstName == firstName).FirstOrDefaultAsync();
        if (myUser != null)
        {
            Console.WriteLine(UserSummary.Assemble(myUser).userSummary.emailAddress);
            return UserSummary.Assemble(myUser);
        }
        return null;
    }

    // public async Task<UserSummary?> GetActionsAi(ArraySegment<String> actions)
    // {
    //     var myUser = await db.Users.Include(x => x.UserPermission).Where(x => x.FirstName == firstName).FirstOrDefaultAsync();
    //     if (myUser != null)
    //     {
    //         Console.WriteLine(UserSummary.Assemble(myUser).userSummary.emailAddress);
    //         return UserSummary.Assemble(myUser);
    //     }
    //     return null;
    // }

    public async Task<Guid> AddUser(UserInfo userInfo)
    {
        using (var dbT = db.Database.BeginTransaction())
        {
            try
            {

                var permission = db.Permissions.FirstOrDefault(p => p.PermissionLevel == userInfo.PermissionLevel);
                if (permission == null)
                {
                    throw new Exception("Couldn't find Permissions");
                }
                User newUser = new User()
                {
                    UserId = Guid.NewGuid(),
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    EmailAddress = userInfo.EmailAddress,
                    Username = userInfo.Username,
                    IsActive = true,
                    UserPermission = permission,

                };
                newUser.PasswordHash = this.EncodePassword(newUser.UserId, userInfo.Password); //we are currently not going to have passwords configured from UserInfo

                //check if email is already used
                if ((await db.Users.Include(u => u.UserPermission).Where(x => x.EmailAddress == newUser.EmailAddress).CountAsync()) > 0)
                {
                    throw new Exception("Email address already in use");
                }

                //check if Username is already used
                if ((await db.Users.Include(u => u.UserPermission).Where(x => x.Username == newUser.Username).CountAsync()) > 0)
                {
                    throw new Exception("Username already in use");
                }

                db.Users.Add(newUser);


                //Console.WriteLine(newUser);
                await db.SaveChangesAsync();

                //TODO: send validation email
                //if ((userInfo.DontSendEmail == null || userInfo.DontSendEmail == false) && !await SendValidationEmail(newUser, false))
                //{
                //    throw new Exception("Couldn't send validation email");
                //}
                //else if (userInfo.DontSendEmail == true)
                //{
                //    var validateEmail = new Email();
                //    validateEmail.EmailID = Guid.NewGuid();
                //    validateEmail.UserID = newUser.UserId;
                //    validateEmail.TypeEnum = (int)EmailType.PasswordReset;
                //    validateEmail.ExpirationDate = DateTimeOffset.UtcNow.AddDays(3);
                //    validateEmail.IsActive = true;
                //    db.Emails.Add(validateEmail);
                //}

                await db.SaveChangesAsync();
                //Console.WriteLine("Changes Saved");
                await dbT.CommitAsync();
                return newUser.UserId;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var mstring = ex.ToString();
                Console.WriteLine(message);
                Console.WriteLine(mstring);
                dbT.Rollback();
                throw new Exception(message);
            }

        }
    }

    // public async Task<UserSummary> GetSelf(HttpContext httpContext){
    //     return await UserSummary.AssembleSelf(httpContext);
    // } 

    public async Task<bool> EditUser(UserSummary userInfo)
    {
        var user = await db.Users.Include(u => u.UserPermission).Where(x => x.UserId == userInfo.userID).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new Exception("Unable to find user");
        }
        var numTimesEmailUsed = await db.Users.Include(u => u.UserPermission).Where(x => x.EmailAddress == userInfo.userSummary.emailAddress).CountAsync();
        if (numTimesEmailUsed > 1 || (numTimesEmailUsed > 0 && userInfo.userSummary.emailAddress != user.EmailAddress))
        {
            throw new Exception("Email address already in use");
        }
        var permission = await db.Permissions.FirstOrDefaultAsync(p => p.PermissionId == (int)userInfo.role);

        if (permission == null)
        {
            throw new Exception("Invalid Permission Level");
        }

        // Step 3: If permission does not exist, create a new one

        user.FirstName = userInfo.userSummary.firstName;
        user.LastName = userInfo.userSummary.lastName;
        user.EmailAddress = userInfo.userSummary.emailAddress;
        user.IsActive = userInfo.userSummary.isActive;
        user.UserPermission = permission;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateUser(LoginInfo loginInfo)
    {
        var user = await db.Users.Include(u => u.UserPermission).FirstOrDefaultAsync(u => u.Username == loginInfo.username && u.IsActive);
        if (user != null)
        {
            if (ValidatePassword(user.UserId, user.PasswordHash, loginInfo.password))
            {
                //user.LastLogin = DateTimeOffset.UtcNow;
                await db.SaveChangesAsync();
                return true;
            }
        }
        Thread.Sleep(1000);
        return false;
    }

    public async Task<bool> Validate(ValidateUserInformation info)
    { //guess we are not going to user this function
        using (var dbT = db.Database.BeginTransaction())
        {
            try
            {
                var emailInfo = await db.Emails.Include(x => x.User).Where(x => x.EmailID == info.validationId).FirstOrDefaultAsync();
                if (emailInfo == null) throw new Exception("Validation request not found");
                if (!emailInfo.IsActive) throw new Exception("Validation request no longer valid");
                if (emailInfo.ExpirationDate <= DateTimeOffset.UtcNow) throw new Exception("Validation request expired");
                if (emailInfo.TypeEnum != (int)EmailType.PasswordReset) throw new Exception("Wrong request type");
                emailInfo.IsActive = false;
                // /emailInfo.User.IsValidated = true;
                emailInfo.User.PasswordHash = EncodePassword(emailInfo.UserID, info.password);

                await db.SaveChangesAsync();

                // if (!(await (new AccountEmailSender().SubmitEmailValidatedEmail(new AccountEmailDetail(emailInfo.User.EmailAddress, null, ValidateUserType.Admin)))))
                // {
                //     dbT.Rollback();

                //     throw new Exception("Couldn't send validated email");
                // }
                await dbT.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                var mstring = ex.ToString();

                dbT.Rollback();

                throw new Exception(message);
            }
        }
    }

    public async Task<bool> SendValidationEmail(User user, bool saveChanges)
    {
        var oldEmails = await db.Emails.Where(x => x.UserID == user.UserId && x.TypeEnum == (int)EmailType.Validation).ToListAsync();
        foreach (var email in oldEmails) email.IsActive = false;
        var validateEmail = new Email();
        validateEmail.EmailID = Guid.NewGuid();
        validateEmail.UserID = user.UserId;
        validateEmail.TypeEnum = (int)EmailType.PasswordReset;
        validateEmail.ExpirationDate = DateTimeOffset.UtcNow.AddDays(3);
        validateEmail.IsActive = true;
        db.Emails.Add(validateEmail);
        await db.SaveChangesAsync();


        //Need to Configure Validation Email
        if (await _emailSender.SubmitNewUserValidateEmail(new AccountEmailDetail(user.EmailAddress, validateEmail.EmailID, ValidateUserType.User)))
        {
            if (saveChanges) await db.SaveChangesAsync();
        }
        else
        {
            Console.WriteLine("Email not sent");
            return false;
        }
        return false;

    }

    public async Task<bool> SendPasswordReset(string email)
    {
        var oldEmails = await db.Emails.Include(x => x.User).Where(x => x.User.EmailAddress == email && x.TypeEnum == (int)EmailType.PasswordReset).ToListAsync();
        foreach (var emailInstance in oldEmails) emailInstance.IsActive = false;
        var user = await db.Users.Include(u => u.UserPermission).Where(x => x.EmailAddress == email).FirstOrDefaultAsync();
        if (user == null)
        {
            Console.WriteLine("User not found");
            return false;
        }

        var resetEmail = new Email();
        resetEmail.EmailID = Guid.NewGuid();
        resetEmail.UserID = user.UserId;
        resetEmail.TypeEnum = (int)EmailType.PasswordReset;
        resetEmail.ExpirationDate = DateTimeOffset.UtcNow.AddDays(3);
        resetEmail.IsActive = true;
        db.Emails.Add(resetEmail);

        //if (await (new AccountEmailSender().SubmitResetPasswordEmail(new AccountEmailDetail(user.EmailAddress, resetEmail.EmailID))))
        //{
        //    await db.SaveChangesAsync();
        //}
        //else
        //{
        //    Console.WriteLine("Email not sent");
        //    return false;
        //}

        return false;
    }

    public async Task<bool> SetPassword(SetPasswordInformation passwordInformation)
    {
        //Console.WriteLine("Reset ID");
        //Console.WriteLine(passwordInformation.resetId);
        var email = await db.Emails.Include(x => x.User).Where(x => x.EmailID == passwordInformation.resetId).FirstOrDefaultAsync();
        if (email == null)
        {
            Console.WriteLine("Reset request not found");
            throw new Exception("Reset request not found");
        }
        if (!email.IsActive)
        {
            Console.WriteLine("Reset request no longer valid");
            throw new Exception("Reset request no longer valid");
        }
        if (email.ExpirationDate <= DateTimeOffset.UtcNow)
        {
            Console.WriteLine("Reset request has expired");
            throw new Exception("Reset request has expired");
        }
        if (email.TypeEnum != (int)EmailType.PasswordReset)
        {
            Console.WriteLine("Wrong Type Request");
            throw new Exception("Wrong Type Request");
        }

        var user = email.User;
        if (user == null)
        {
            Console.WriteLine("User not found");
            throw new Exception("User not found");
        }

        email.IsActive = false;
        user.PasswordHash = EncodePassword(user.UserId, passwordInformation.password);
        //if(!user.IsValidated) user.IsValidated = true;

        await db.SaveChangesAsync();
        return true;

    }

    private bool ValidatePassword(Guid userId, string userPassword, string enteredPassword)
    {
        if (EncodePassword(userId, enteredPassword) == userPassword)
        {
            return true;
        }

        return false;
    }

    public string EncodePassword(Guid userId, string originalPassword)
    {
        var id = userId.GetHashCode();

        var part1 = id & 0xDDDDDDDD;
        var part2 = id & 0x33333333;
        if (part1 == 0)
        {
            part1 = id;
        }
        if (part2 == 0)
        {
            part2 = id;
        }

        var salt = part1 * part2;

        return GetHash(salt.ToString() + originalPassword);
    }
    public string GetHash(string strData)
    {
        var message = Encoding.UTF8.GetBytes(strData);
        using (var alg = SHA256.Create())
        {
            string hex = "";

            var hashValue = alg.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }


}
