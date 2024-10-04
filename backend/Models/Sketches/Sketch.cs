using Azure.Storage.Blobs.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Probably need to rework this data structure, make sure this is compliant with Entity Frameworks
public class Sketch {

    [Key]
    public int PostId { get; set; }

    [Required]
    public Guid ArtistID { get; set; }

    [ForeignKey("UserId")]
    public User? user { get; set; }
    public string? MediaUrl { get; set; }

    // Prompt

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Comment> Comments { get; set; }

    public ICollection<Like> Likes { get; set; }

    public bool IsArchived { get; set; } = false;

   // public ICollection<PostTag> PostTags { get; set; }


}
