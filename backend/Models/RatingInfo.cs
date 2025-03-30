using System.ComponentModel.DataAnnotations;

public class RatingInfo
{
    public int? Id { get; set; } 

    [Required]
    public int PromptId { get; set; }

    [Required]
    public RatingType RatingValue { get; set; } 

    public string? Comment { get; set; }
}

