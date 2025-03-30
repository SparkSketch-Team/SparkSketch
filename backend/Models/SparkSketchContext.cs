using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

public class SparkSketchContext : DbContext
{
    private string connectionString;

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Prompt> Prompts { get; set; } = null!;
    public DbSet<Permission> Permissions { get; set; } = null!;
    public virtual DbSet<Email> Emails { get; set; } = null!;
    public DbSet<Sketch> Sketches { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Like> Likes { get; set; } = null!;
    public DbSet<Follower> Followers { get; set; } = null!;
    public DbSet<Media> Media { get; set; } = null!;
    public DbSet<Rating> Ratings { get; set; } = null!;


    public SparkSketchContext(IConfiguration configuration)
    {
        var envConnectionString = Environment.GetEnvironmentVariable("SQL_CONNECTION_STRING");
        connectionString = envConnectionString ?? configuration.GetConnectionString("DefaultConnection");
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        Console.WriteLine($"Using connection string: {connectionString}");
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
