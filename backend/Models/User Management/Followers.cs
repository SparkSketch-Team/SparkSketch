using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Follower
{
    [Key]
    public int FollowerID { get; set; }

    [Required]
    public int UserID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }

    [Required]
    public int FollowingUserID { get; set; }

    [ForeignKey("FollowingUserID")]
    public User FollowingUser { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
