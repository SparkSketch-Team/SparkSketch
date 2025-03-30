using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Rating
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PromptId { get; set; } // Foreign key linking to Prompt

    [Required]
    public RatingType RatingValue { get; set; } // Enum for Good, Neutral, Bad

    public string? Comment { get; set; } // Optional comment

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Prompt Prompt { get; set; } // Navigation property
}


[JsonConverter(typeof(JsonStringEnumConverter))]
public enum RatingType
{
    Good,
    Neutral,
    Bad
}


