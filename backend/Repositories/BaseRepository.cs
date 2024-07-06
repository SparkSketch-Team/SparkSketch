public abstract class BaseRepository : IDisposable
{
    protected SparkSketchContext db { get; private set; }

    public BaseRepository()
    {
        db = new SparkSketchContext();
    }

    
    public void Dispose()
    {
        db.Dispose();
    }
    
}