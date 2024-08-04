using System.ComponentModel.DataAnnotations;

public class Role
{
    [Key]
    public Guid RoleId { get; set; }

    [Required]
    public String RoleName { get; set; } = null!;
}