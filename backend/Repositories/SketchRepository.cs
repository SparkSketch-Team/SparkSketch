using backend.Services;
using Microsoft.EntityFrameworkCore;

public class SketchRepository : BaseRepository, ISketchRepository
{
    public SketchRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Sketch> GetSketchByIdAsync(int postId)
    {
        var sketch = await db.Sketches
            .Include(s => s.user)
            .Include(s => s.Comments) 
            .Include(s => s.Likes) 
            .FirstOrDefaultAsync(s => s.PostId == postId);

        if (sketch == null)
        {
            throw new Exception("Sketch not found");
        }

        return sketch;
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

    // This probably needs to be reworked
     public async Task<bool> DeleteSketchAsync(int postId) 
    {
        var sketch = await db.Sketches.FindAsync(postId);
        if (sketch == null)
        {
            return false;
        }

        db.Sketches.Remove(sketch);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ArchiveSketchAsync(int postId)
    {
        var sketch = await db.Sketches.FindAsync(postId);
        if (sketch == null)
        {
            return false;
        }

        sketch.IsArchived = true;
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Sketch>> GetSketchesByUsernameAsync(string username)
    {
        return await db.Sketches
                       .Join(db.Users,
                             s => s.ArtistID,
                             u => u.UserId,
                             (s, u) => new { Sketch = s, User = u })
                       .Where(joined => joined.User.Username.Contains(username))
                       .Select(joined => joined.Sketch)
                       .ToListAsync();
    }

    public async Task<User?> GetUserBySketchPostId(int postId)
    {
        return await db.Sketches
                       .Where(s => s.PostId == postId)
                       .Join(db.Users,
                             s => s.ArtistID,
                             u => u.UserId,
                             (s, u) => u)
                       .FirstOrDefaultAsync();
    }

}