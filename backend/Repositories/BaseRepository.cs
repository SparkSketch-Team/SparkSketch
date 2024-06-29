public abstract class BaseRepository : IDisposable
{
    //protected DATABASE_NAME db { get; private set; }

    public BaseRepository()
    {
        //db = new CoordinatusContext();
    }

    
    public void Dispose()
    {
        //db.Dispose();
    }
    
}