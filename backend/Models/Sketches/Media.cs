using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Media
{
    [Key]
    public int MediaID { get; set; }

    [Required]
    public Guid ArtistID { get; set; }

    [ForeignKey("UserID")]
    public User User { get; set; }

    public string MediaUrl { get; set; }

    public string Type { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
