public class UserInfo
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string EmailAddress { get; set; } = null!;
    //public string? Password { get; set; } we are currently not going to have passwords configured from UserInfo
    public bool IsActive { get; set; }
    public string PermissionLevel { get; set; } = null!;
    public bool? dontSendEmail { get; set; }
}