using MisaBE.Common.DTOs;
using MisaBE.DL.Base;

namespace MisaBE.BL.Base
{
    public abstract class BaseBL<T> : IBaseBL<T>
    {
        protected readonly IBaseDL<T> DL;

        protected BaseBL(IBaseDL<T> dl)
        {
            DL = dl;
        }

        public virtual async Task<PagingResult<T>> GetPagedAsync(PagingRequest request)
            => await DL.GetPagedAsync(request);

        public virtual async Task<T?> GetByIdAsync(string id)
            => await DL.GetByIdAsync(id);

        public virtual async Task<ServiceResult> InsertAsync(T entity)
        {
            var errors = await ValidateAsync(entity, isInsert: true);
            if (errors.Any())
                return ServiceResult.Fail("Dữ liệu không hợp lệ", errors);

            var id = await DL.InsertAsync(entity);
            return ServiceResult.Ok(id, "Thêm mới thành công");
        }

        public virtual async Task<ServiceResult> UpdateAsync(string id, T entity)
        {
            var errors = await ValidateAsync(entity, isInsert: false, id: id);
            if (errors.Any())
                return ServiceResult.Fail("Dữ liệu không hợp lệ", errors);

            var affected = await DL.UpdateAsync(entity);
            if (affected == 0)
                return ServiceResult.Fail("Không tìm thấy bản ghi");

            return ServiceResult.Ok(null, "Cập nhật thành công");
        }

        public virtual async Task<ServiceResult> DeleteAsync(string id)
        {
            var affected = await DL.DeleteAsync(id);
            if (affected == 0)
                return ServiceResult.Fail("Không tìm thấy bản ghi");
            return ServiceResult.Ok(null, "Xóa thành công");
        }

        public virtual async Task<ServiceResult> DeleteMultipleAsync(IEnumerable<string> ids)
        {
            var idList = ids.ToList();
            if (!idList.Any())
                return ServiceResult.Fail("Không có bản ghi nào được chọn");

            var affected = await DL.DeleteMultipleAsync(idList);
            return ServiceResult.Ok(affected, $"Đã xóa {affected} bản ghi");
        }

        /// <summary>Override to add entity-specific validation. Return a list of error messages.</summary>
        protected virtual Task<List<string>> ValidateAsync(T entity, bool isInsert, string? id = null)
            => Task.FromResult(new List<string>());
    }
}
