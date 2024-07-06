using System.ComponentModel.DataAnnotations;

public class Prompt
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string Theme { get; set; }
    [Required]
    public string PromptText { get; set; } // Rename "prompt" to "PromptText" to avoid naming conflicts
    [Required]
    public bool IsActive { get; set; }
    public DateTime PromptDate { get; set; }
    [Required]
    public DateTime CreateDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
}