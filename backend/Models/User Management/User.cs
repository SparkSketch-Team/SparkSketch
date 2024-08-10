using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Hosting;

public class User
{
    [Key]
    public Guid UserId { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;
    [Required]
    public string LastName { get; set; } = null!;
    [Required]
    public string Username { get; set; } = null!;
    [Required]
    public string EmailAddress { get; set; } = null!;
    public string PasswordHash { get; set; } = "Password"; // The issue is is that this leaves a default password when we create an user. Ideally, we have a null password so the unvalidated user cannot access
    [Required]
    public bool IsActive { get; set; }
    public Permission UserPermission { get; set; }

    public string ProfilePictureUrl { get; set; } = null!;

    public string Bio { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Last Login

    public ICollection<Sketch> Sketches { get; set; }

    public ICollection<Comment> Comments { get; set; }

    public ICollection<Like> Likes { get; set; }

    public ICollection<Follower> Followers { get; set; }

    public ICollection<Follower> Following { get; set; }

    public ICollection<Media> Media { get; set; }
}
