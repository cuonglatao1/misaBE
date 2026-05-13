using MisaBE.Common.DTOs;

namespace MisaBE.DL.Base
{
    public interface IBaseDL<T>
    {
        Task<PagingResult<T>> GetPagedAsync(PagingRequest request);
        Task<T?> GetByIdAsync(string id);
        Task<string> InsertAsync(T entity);
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(string id);
        Task<int> DeleteMultipleAsync(IEnumerable<string> ids);
        Task<bool> IsCodeExistsAsync(string code, string? excludeId = null);
    }
}
