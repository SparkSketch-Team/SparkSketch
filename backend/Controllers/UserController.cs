using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiController
{
    private UserRepository uRepo;

    public UserController(IConfiguration configuration, ILogger<AiController> logger) : base(logger)
    {
        uRepo = new UserRepository(configuration);
    }

    [HttpPost]
    [Route("Login")]
    public async Task<JsonResult> Login([FromBody] LoginInfo loginInfo)
    {
        //Console.WriteLine("Login");
        var context = HttpContext;

        Random random = new Random();
        await Task.Delay(random.Next(1000, 5000));

        if (await uRepo.ValidateUser(loginInfo))
        {
            var claims = await uRepo.GetUserClaims(loginInfo);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)
            );

            var user = context.User;
            var claimsT = context.User.Claims;
            var uid = context.User.Claims.FirstOrDefault(c => c.Type == CoordinatwosClaims.UserId);

            return SuccessMessage(true);
        }
        else
        {
            //Console.WriteLine("Not Validated");
            return FailMessage();
        }

    }

    // [HttpGet]
    // [Route("GetSelf")]
    // public async Task<JsonResult> GetSelf(){
    //     using (aRepo){
    //         try {
    //             var response = await aRepo.GetSelf(HttpContext);
    //             if(response == null) {
    //                 return FailMessage();
    //             }
    //             return SuccessMessage(response);
    //         } catch (Exception ex) {
    //             return FailMessage(ex.Message);
    //         }
    //     }
    // }

    [HttpPost]
    [Route("AddUser")]
    public async Task<JsonResult> AddUser([FromBody] UserInfo userInfo)
    {
        try
        {
            var response = await uRepo.AddUser(userInfo);
            if (response != null)
            {
                return SuccessMessage(response);
            }
            else
            {
                return FailMessage("Adding User Failed");
            }
        }
        catch (Exception ex)
        {
            return FailMessage(ex.Message);
        }
    }

    [HttpPost]
    //[Authorize(Policy = "AdminOnly")]
    [Route("EditUser")]
    public async Task<JsonResult> EditUse([FromBody] UserSummary info)
    {
        using (uRepo)
        {
            try
            {
                var response = await uRepo.EditUser(info);
                if (response)
                {
                    return SuccessMessage(true);
                }
                else
                {
                    return FailMessage("Edit User Failed");
                }
            }
            catch (Exception ex)
            {
                return FailMessage(ex.Message);
            }
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("Validate")]
    public async Task<JsonResult> Validate([FromBody] ValidateUserInformation info)
    {
        using (uRepo)
        {
            try
            {
                var response = await uRepo.Validate(info);
                if (response)
                {
                    return SuccessMessage(true);
                }
                else
                {
                    return FailMessage("Validating User Failed");
                }
            }
            catch (Exception ex)
            {
                return FailMessage(ex.Message);
            }
        }
    }

    [HttpGet]
    [AllowAnonymous]
    [Route("SendPasswordReset")]
    public async Task<JsonResult> SendPasswordReset(string email)
    {
        try
        {
            var response = await uRepo.SendPasswordReset(email);
            if (!response) return FailMessage("Send reset email failed");
            return SuccessMessage(response);
        }
        catch (Exception ex)
        {
            return FailMessage(ex.Message);
        }
    }

    [HttpPost]
    [AllowAnonymous]
    [Route("ResetPassword")]
    public async Task<JsonResult> ResetPassword([FromBody] SetPasswordInformation passwordInformation)
    {
        try
        {
            var response = await uRepo.SetPassword(passwordInformation);
            if (!response) return FailMessage("Setting password failed");
            return SuccessMessage(response);

        }
        catch (Exception ex)
        {
            return FailMessage(ex.Message);
        }
    }

    [HttpPost]
    //[Authorize(Policy = "AdminOnly")]
    [Route("GetUsersDT")]
    public async Task<JsonResult> GetUsersDT([FromBody] UserDTRequest dtRequest)
    {
        using (uRepo)
        {
            return SuccessMessage(await uRepo.GetUsersDT(dtRequest));
        }
    }

    [HttpGet]
    //[Authorize(Policy = "AdminUser")]
    [Route("GetUser")]
    public async Task<JsonResult> GetUser(Guid userID)
    {
        using (uRepo)
        {
            return SuccessMessage(await uRepo.GetUser(userID));
        }
    }


    [HttpGet]
    [Route("Logout")]
    public async Task<JsonResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return SuccessMessage(true);
    }

    // [HttpPost]
    // //[Authorize(Policy = "AdminUser")]
    // [Route("HelpUserAi")]
    // public async Task<JsonResult> GetUser([FromBody] ArraySegment<String> actions){
    //     using (uRepo) {
    //         return SuccessMessage(await uRepo.GetUserAi(actions));
    //     }   
    // }






}