public interface ILikeRepository
{
    Task<Like> CreateLikeAsync(Like like);
    Task<bool> RemoveLikeAsync(int likeId);
    Task<List<Like>> GetLikesByPostIdAsync(int postId);
    Task<bool> RemoveLikeAsync(int postId, Guid userId);
}
