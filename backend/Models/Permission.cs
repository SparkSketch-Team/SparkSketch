using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

// public class PermisionLevel : DbContext
// {
//     // public DbSet<User> permisionLevels { get; set; }
//     // // public DbSet<Role> roles {get; set; }
//     // // public DbSet<PermisionLevel> permisions {get; set; }

//     // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//     // {
//     //     optionsBuilder.UseSqlServer("Server=localhost;Database=Coordinatus-database;Trusted_Connection=True;Encrypt=False");
//     // }

// }

public class Permission
{
    [Key]
    public int PermissionId { get; set; }
    [Required]
    public String PermissionLevel { get; set; } = null!;
}