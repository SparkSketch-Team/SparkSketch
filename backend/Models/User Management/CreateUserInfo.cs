public class CreateUserInfo
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsActive { get; set; }
    public string PermissionLevel { get; set; } = null!;
    public bool? DontSendEmail { get; set; }
}