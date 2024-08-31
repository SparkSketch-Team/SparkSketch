public interface ISketchRepository
{
    Task<Sketch> CreateSketchAsync(Sketch sketch);
    Task<List<Sketch>> GetSketchesByUserIdAsync(Guid userId);
    Task<List<Sketch>> GetAllSketchesAsync();
    Task<bool> DeleteSketchAsync(int postId);
    Task<List<Sketch>> GetSketchesByUsernameAsync(string username);
    Task<Sketch> GetSketchByIdAsync(int postId);
    Task<User?> GetUserBySketchPostId(int postId);
}
