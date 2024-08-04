using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class SparkSketchContext : DbContext
{
    private IConfiguration _config;
    private string connectionString;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Prompt> Prompts { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public virtual DbSet<Email> Emails { get; set; } = null!;

    public SparkSketchContext(IConfiguration configuration)
    {
        _config = configuration;
        connectionString = _config.GetConnectionString("DefaultConnection");
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //TODO: Fix, huge security violation and is retarded
        optionsBuilder.UseSqlServer(connectionString);
    }


}
