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
    public async Task<JsonResult> AddUser([FromBody] CreateUserInfo userInfo)
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
    [Route("EditUser")]
    [Authorize]
    public async Task<JsonResult> EditUser([FromBody] EditUserInfo info)
    {
        try
        {
            // Get the current user's ID from the JWT claims
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;

            if (currentUserId is null || currentUserId.IsNullOrEmpty())
            {
                return FailMessage("Unauthorized attempt to edit another user's profile.");
            }

            var response = await _userRepository.EditUser(info, currentUserId);
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

    [HttpGet]
    [Authorize]
    [Route("GetSelf")]
    public async Task<JsonResult> GetSelf()
    {
        try
        {
            // Get the current user's ID from the JWT claims
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == SparkSketchClaims.UserId)?.Value;

            if (currentUserId is null || currentUserId.IsNullOrEmpty())
            {
                return FailMessage("There is no user to get.");
            }

            var response = await _userRepository.GetSelf(currentUserId);
            if (response != null)
            {
                return SuccessMessage(response);
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

    [HttpGet]
    [Authorize]
    [Route("GetUserById")]
    public async Task<JsonResult> GetUserById([FromQuery] Guid userId)
    {
        try
        {
            // Attempt to retrieve the user by their ID
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return FailMessage("User not found");
            }

            return SuccessMessage(user);
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


}