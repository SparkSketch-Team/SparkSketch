using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Follower
{
    [Key]
    public int FollowID { get; set; }

    [Required]
    public Guid FollowedUserID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }

    [Required]
    public Guid FollowingUserID { get; set; }

    [ForeignKey("FollowingUserID")]
    public User FollowingUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
