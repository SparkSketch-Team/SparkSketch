using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Extensions.Hosting;

public class Comment
{
    [Key]
    public int CommentID { get; set; }

    [Required]
    public int PostID { get; set; }

    [ForeignKey("PostID")]
    public Sketch Sketch { get; set; }

    [Required]
    public Guid CommenterID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
