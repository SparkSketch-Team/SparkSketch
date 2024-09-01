public interface ICommentRepository
{
    Task<Comment> CreateCommentAsync(Comment comment);
    Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
    Task<List<Comment>> GetCommentsByUserIdAsync(Guid userId);
    Task<bool> DeleteCommentAsync(int commentId);
}
