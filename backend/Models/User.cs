using System.ComponentModel.DataAnnotations;
using System.Data;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public string EmailAddress { get; set; } = null!;
    public string PasswordHash { get; set; } = "Password"; // The issue is is that this leaves a default password when we create an user. Ideally, we have a null password so the unvalidated user cannot access
    [Required]
    public bool IsActive { get; set; }
    public Permission UserPermission { get; set; }
}
