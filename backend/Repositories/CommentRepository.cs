using Microsoft.EntityFrameworkCore;

public class CommentRepository : BaseRepository, ICommentRepository
{
    public CommentRepository(IConfiguration config) : base(config)
    {
    }

    public async Task<Comment> CreateCommentAsync(Comment comment)
    {
        db.Comments.Add(comment);
        await db.SaveChangesAsync();
        return comment;
    }

    public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
    {
        return await db.Comments
                        .Where(c => c.PostID == postId)
                        .ToListAsync();
    }

    public async Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId)
    {
        return await db.Comments
                        .Where(c => c.CommenterID == userId)
                        .ToListAsync();
    }

    public async Task<bool> DeleteCommentAsync(int commentId)
    {
        var comment = await db.Comments.FindAsync(commentId);
        if (comment == null)
        {
            return false;
        }

        db.Comments.Remove(comment);
        await db.SaveChangesAsync();
        return true;
    }
}
