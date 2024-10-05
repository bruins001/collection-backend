using collection_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace collection_backend.Repositories
{
    public interface IGenericRespository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>?> GetBulkByIdAsync(IEnumerable<int> ids);
        Task<T?> GetOneByIdAsync(int id);
        Task<T> InsertOneAsync(T tool);
        Task<IEnumerable<T>> InsertBulkAsync(IEnumerable<T> tools);
        Task<T> UpdateOneAsync(T tool);
        Task<IEnumerable<T>> UpdateBulkAsync(IEnumerable<T> tools);
        Task DeleteOneByIdAsync(int id);
        Task DeleteBulkByIdAsync(int[] id);
    }
}
