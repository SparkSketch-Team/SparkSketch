using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class SparkSketchContext : DbContext
{
    private IConfiguration _config;
    private string connectionString;

    public DbSet<Prompt> Prompts { get; set; } = null!;

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
