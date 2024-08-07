using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

public partial class Email
{
    [Key]
    public Guid EmailID { get; set; }
    public virtual User User { get; set; } = null!;
    [Required]
    [ForeignKey("User")]
    public Guid UserID { get; set; }
    [Required]
    public int TypeEnum { get; set; }
    [Required]
    public bool IsActive { get; set; }
    [Required]
    public DateTimeOffset ExpirationDate { get; set; }

}

public enum EmailType
{
    Validation = 0,
    PasswordReset = 1
}