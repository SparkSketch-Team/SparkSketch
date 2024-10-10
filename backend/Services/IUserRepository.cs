using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

public interface IUserRepository
{
    Task<Claim[]> GetUserClaims(LoginInfo loginInfo);
    Task<string> Login(LoginInfo loginInfo);
    Task<string> RegisterAndLoginUser(CreateUserInfo userInfo);
    Task<Guid> AddUser(CreateUserInfo userInfo);
    Task<bool> EditUser(EditUserInfo userInfo, string userId, string? profilePictureUrl);
    Task<EditUserInfo> GetSelf(string userId);
    Task<User> GetUserByIdAsync(Guid userId);
    Task<bool> ValidateUser(LoginInfo loginInfo);
    Task<bool> Validate(ValidateUserInformation info);
    Task<bool> SendValidationEmail(User user, bool saveChanges);
    Task<bool> SendPasswordReset(string email);
    Task<bool> SetPassword(SetPasswordInformation passwordInformation);
    string EncodePassword(Guid userId, string originalPassword);
    string GetHash(string strData);
}

