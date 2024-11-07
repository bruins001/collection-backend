using collection_backend.Models;

namespace collection_backend.Repositories
{
    public interface IToolRepository
    {
        Task<IEnumerable<Tool>> GetAllAsync();
        Task<IEnumerable<Tool>?> GetBulkByIdAsync(IEnumerable<int> ids);
        Task<Tool?> GetOneByIdAsync(int id);
        Task<Tool> InsertOneAsync(Tool tool);
        Task<IEnumerable<Tool>> InsertBulkAsync(IEnumerable<Tool> tools);
        Task<Tool> UpdateOneAsync(Tool tool);
        Task<IEnumerable<Tool>> UpdateBulkAsync(IEnumerable<Tool> tools);
        Task DeleteOneByIdAsync(int id);
        Task DeleteBulkByIdAsync(int[] id);
    }
}
