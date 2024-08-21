public interface ISketchRepository
{
    Task<Sketch> CreateSketchAsync(Sketch sketch);
    Task<List<Sketch>> GetSketchesByUserIdAsync(Guid userId);
    Task<List<Sketch>> GetAllSketchesAsync();
}
