using backend.Services;
using Microsoft.EntityFrameworkCore;

public class SketchRepository : BaseRepository, ISketchRepository
{
    public SketchRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Sketch> CreateSketchAsync(Sketch sketch)
    {
        db.Sketches.Add(sketch);
        await db.SaveChangesAsync();
        return sketch;
    }

    public async Task<List<Sketch>> GetSketchesByUserIdAsync(Guid userId)
    {
        return await db.Sketches
                               .Where(s => s.ArtistID == userId)
                               .ToListAsync();
    }

    public async Task<List<Sketch>> GetAllSketchesAsync() 
    {
        return await db.Sketches.ToListAsync();
    }
}