
using Org.BouncyCastle.Asn1.Mozilla;

public class BaseUserSummary
{
    public Guid userId { get; set; }
    public string firstName { get; set; } = null!;
    public string lastName { get; set; } = null!;
    public string fullName { get; set; } = null!;
    public string emailAddress { get; set; } = null!;
    public bool isActive { get; set; }
    public string username { get; set; } = null!;
    public string profilePictureUrl { get; set; } = null!;
    public string bio { get; set; } = null!;

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
            isActive = user.IsActive,
            emailAddress = user.EmailAddress,
            username = user.Username,
            profilePictureUrl = user.ProfilePictureUrl,
            bio = user.Bio
            //we currently do not support Avatar URL
            //we currently do not support Last Login
            //we currently do not support IsValidated

        };
    }
}