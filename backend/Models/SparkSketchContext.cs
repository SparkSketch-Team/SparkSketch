using Microsoft.EntityFrameworkCore;

public class SparkSketchContext : DbContext
{

    public DbSet<Prompt> prompts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional configuration here
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(SparkSketchConfig.Instance.ConnectionStringDefaultConnection);
    }


}
