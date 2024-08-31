using Microsoft.EntityFrameworkCore;

public class LikeRepository : BaseRepository, ILikeRepository
{
    public LikeRepository(IConfiguration config) : base(config)
    {
    }

    public async Task<Like> CreateLikeAsync(Like like)
    {
        db.Likes.Add(like);
        await db.SaveChangesAsync();
        return like;
    }

    public async Task<bool> RemoveLikeAsync(int likeId)
    {
        var like = await db.Likes.FindAsync(likeId);
        if (like == null)
        {
            return false;
        }

        db.Likes.Remove(like);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveLikeAsync(int postId, Guid userId)
    {
        var like = await db.Likes.FirstOrDefaultAsync(l => l.PostID == postId && l.UserID == userId);
        if (like == null)
        {
            return false;
        }

        db.Likes.Remove(like);
        await db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Like>> GetLikesByPostIdAsync(int postId)
    {
        return await db.Likes
                        .Where(l => l.PostID == postId)
                        .ToListAsync();
    }
}
