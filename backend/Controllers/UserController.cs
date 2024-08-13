using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.CodeDom.Compiler;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _config;

    public UserController(IUserRepository userRepository, IConfiguration config, ILogger<UserController> logger) : base(logger)
    {
        _userRepository = userRepository;
        _config = config;
    }

    [HttpPost]
    [Route("Login")]
    public async Task<JsonResult> Login([FromBody] LoginInfo loginInfo)
    {
        var token = await _userRepository.Login(loginInfo);
        if (!string.IsNullOrEmpty(token))
        {
            return SuccessMessage(token);
        }
        else
        {
            return FailMessage("Failed to Login");
        }
    }

    [HttpPost]
    [Route("AddUser")]
    public async Task<JsonResult> AddUser([FromBody] UserInfo userInfo)
    {
        try
        {
            var token = await _userRepository.RegisterAndLoginUser(userInfo);
            if (!string.IsNullOrEmpty(token))
            {
                return SuccessMessage(token);
            }
            else
            {
                return FailMessage("Failed to Login");
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
    public async Task<JsonResult> EditUser([FromBody] UserSummary info)
    {
            try
            {
                var response = await _userRepository.EditUser(info);
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

    [HttpPost]
    [AllowAnonymous]
    [Route("Validate")]
    public async Task<JsonResult> Validate([FromBody] ValidateUserInformation info)
    {
            try
            {
                var response = await _userRepository.Validate(info);
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

    [HttpGet]
    [AllowAnonymous]
    [Route("SendPasswordReset")]
    public async Task<JsonResult> SendPasswordReset(string email)
    {
        try
        {
            var response = await _userRepository.SendPasswordReset(email);
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
            var response = await _userRepository.SetPassword(passwordInformation);
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
        return SuccessMessage(await _userRepository.GetUsersDT(dtRequest));
    }

    [HttpGet]
    //[Authorize(Policy = "AdminUser")]
    [Route("GetUser")]
    public async Task<JsonResult> GetUser(Guid userID)
    {

            return SuccessMessage(await _userRepository.GetUser(userID));
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