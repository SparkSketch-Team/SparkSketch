using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

public class Like
{
    [Key]
    public int LikeID { get; set; }

    [Required]
    public int PostID { get; set; }

    [ForeignKey("PostID")]
    public Sketch Post { get; set; }

    [Required]
    public int UserID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
