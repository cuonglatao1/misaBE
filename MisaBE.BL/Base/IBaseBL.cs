using MisaBE.Common.DTOs;

namespace MisaBE.BL.Base
{
    public interface IBaseBL<T>
    {
        Task<PagingResult<T>> GetPagedAsync(PagingRequest request);
        Task<T?> GetByIdAsync(string id);
        Task<ServiceResult> InsertAsync(T entity);
        Task<ServiceResult> UpdateAsync(string id, T entity);
        Task<ServiceResult> DeleteAsync(string id);
        Task<ServiceResult> DeleteMultipleAsync(IEnumerable<string> ids);
    }
}
