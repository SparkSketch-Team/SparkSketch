using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

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
    public DbSet<Sketch> Sketchs { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Follower> Followers { get; set; }
    public DbSet<Media> Media { get; set; }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Follower>()
            .HasOne(f => f.User)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowedUserID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follower>()
            .HasOne(f => f.FollowingUser)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowingUserID)
            .OnDelete(DeleteBehavior.Restrict);
    }


}
