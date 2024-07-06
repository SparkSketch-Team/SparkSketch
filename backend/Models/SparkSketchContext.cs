using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

public class SparkSketchContext : DbContext
{

    public DbSet<Prompt> Prompts { get; set; } = null!;


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        //TODO: Fix, huge security violation and is retarded
        optionsBuilder.UseSqlServer("Server=tcp:sparksketch-dev.database.windows.net,1433;Initial Catalog=SparkSketch;Persist Security Info=False;User ID=hueyee;Password=Astolfofan27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30");
    }


}
