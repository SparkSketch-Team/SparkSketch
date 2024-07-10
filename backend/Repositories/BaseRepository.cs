public abstract class BaseRepository : IDisposable
{
    protected SparkSketchContext db { get; private set; }
    protected IConfiguration _config {  get; set; }

    public BaseRepository(IConfiguration config)
    {
        db = new SparkSketchContext(config);
        _config = config;
    }

    
    public void Dispose()
    {
        db.Dispose();
    }
    
}