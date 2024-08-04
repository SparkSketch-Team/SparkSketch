
public class UserSummary
{
    public Guid userID { get; set; }
    public UserType role { get; set; }
    public BaseUserSummary userSummary { get; set; } = null!;

    public static UserSummary Assemble(User user)
    {
        return new UserSummary()
        {
            userID = user.UserId,
            role = (int)user.UserPermission.PermissionId == 1 ? UserType.Admin : UserType.User,
            userSummary = BaseUserSummary.Assemble(user)
        };
    }

    // public static async Task<UserSummary> AssembleSelf(HttpContext httpContext) {
    //     var db = new CoordinatusContext();
    //     var userTypeClaim = getClaim(httpContext, CoordinatwosClaims.UserType);
    //     if(userTypeClaim == null) {
    //         return null;
    //     }
    //     UserType userType = (UserType)int.Parse(userTypeClaim);
    //     UserSummary returnedUser = null;
    //     //this is where we do differnet stuff depending on which user, however I am not going to do that right now
    //     //for now I am going to use a generic user, future developer will have to figure it out
    //     //here is where to do that https://bitbucket.org/counterpart-biz/srs-portal-code/src/2affa8c2fe75d204a3d2816adbfa14ef9a8aa6b5/SRSInfrastructure/SRS/UserManagement/User.cs?at=master#User.cs-33
    //     var userId = getClaim(httpContext, CoordinatwosClaims.UserId);
    //     var user = await db.Users.Where(x => x).FirstOrDefaultAsync( x => x.UserId.ToString() == (userId && x.IsActive));
    // }

    // public static string getClaim(HttpContext httpContext, string claimType) {
    //     var claim = httpContext.User.Claims.FirstOrDefault(c => c.Type == claimType);
    //     if(claim == null){
    //         return null;
    //     }
    //     return claim.Value;
    // }
}

public enum UserType
{
    Admin = 1,
    User = 2
}

