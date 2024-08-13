using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public interface IUserRepository
{
    Task<Claim[]> GetUserClaims(LoginInfo loginInfo);
    Task<string> Login(LoginInfo loginInfo);
    Task<string> RegisterAndLoginUser(UserInfo userInfo);
    Task<UserDTSummary> GetUsersDT(UserDTRequest userDTRequest);
    Task<UserSummary?> GetUser(Guid userId);
    Task<UserSummary?> GetUserAi(string firstName);
    Task<Guid> AddUser(UserInfo userInfo);
    Task<bool> EditUser(UserSummary userInfo);
    Task<UserSummary> GetSelf(HttpContext httpContext);
    Task<bool> ValidateUser(LoginInfo loginInfo);
    Task<bool> Validate(ValidateUserInformation info);
    Task<bool> SendValidationEmail(User user, bool saveChanges);
    Task<bool> SendPasswordReset(string email);
    Task<bool> SetPassword(SetPasswordInformation passwordInformation);
    string EncodePassword(Guid userId, string originalPassword);
    string GetHash(string strData);
}

