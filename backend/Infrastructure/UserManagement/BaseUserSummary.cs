
public class BaseUserSummary
{
    public Guid userId { get; set; }
    public string firstName { get; set; } = null!;
    public string lastName { get; set; } = null!;
    public string fullName { get; set; } = null!;
    public string initials { get; set; } = null!;
    public string emailAddress { get; set; } = null!;
    public bool isActive { get; set; }
    // public string avatarUrl { get; set; }
    // public DateTimeOffset? lastLogin { get; set; }
    // public bool isValidated { get; set; }

    public static BaseUserSummary Assemble(User user)
    {
        return new BaseUserSummary()
        {
            userId = user.UserId,
            firstName = user.FirstName,
            lastName = user.LastName,
            fullName = user.FirstName + " " + user.LastName,
            initials = (user.FirstName.Length > 0 ? user.FirstName.ToUpper().Substring(0, 1) : "") + (user.LastName.Length > 0 ? user.LastName.ToUpper().Substring(0, 1) : ""),
            isActive = user.IsActive,
            emailAddress = user.EmailAddress,
            //we currently do not support Avatar URL
            //we currently do not support Last Login
            //we currently do not support IsValidated

        };
    }
}