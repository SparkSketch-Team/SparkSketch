public class SetPasswordInformation
{
    public Guid resetId { get; set; }
    public string password { get; set; } = null!;
}

public class ValidateUserInformation
{
    public Guid validationId { get; set; }
    public string password { get; set; } = null!;
}