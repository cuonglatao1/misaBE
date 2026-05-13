using Microsoft.AspNetCore.Mvc;
using MisaBE.API.Controllers.Base;
using MisaBE.BL.SalaryComposition;
using MisaBE.Common.DTOs;
using MisaBE.Common.Entities;

namespace MisaBE.API.Controllers
{
    [Route("api/salary-compositions")]
    public class SalaryCompositionController : BaseController
    {
        private readonly ISalaryCompositionBL _bl;

        public SalaryCompositionController(ISalaryCompositionBL bl) => _bl = bl;

        /// <summary>Lấy danh sách thành phần lương có phân trang, tìm kiếm, lọc</summary>
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] PagingRequest request)
        {
            var result = await _bl.GetPagedAsync(request);
            return Ok(ServiceResult.Ok(result));
        }

        /// <summary>Lấy chi tiết một thành phần lương theo Id</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var entity = await _bl.GetByIdAsync(id);
            if (entity == null) return NotFoundResult();
            return Ok(ServiceResult.Ok(entity));
        }

        /// <summary>Thêm mới thành phần lương</summary>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] SalaryComposition entity)
        {
            var result = await _bl.InsertAsync(entity);
            return HandleResult(result);
        }

        /// <summary>Cập nhật thành phần lương</summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SalaryComposition entity)
        {
            entity.CompositionId = id;
            var result = await _bl.UpdateAsync(id, entity);
            return HandleResult(result);
        }

        /// <summary>Xóa một thành phần lương</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _bl.DeleteAsync(id);
            return HandleResult(result);
        }

        /// <summary>Xóa nhiều thành phần lương (body: mảng Id)</summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteMultiple([FromBody] IEnumerable<string> ids)
        {
            var result = await _bl.DeleteMultipleAsync(ids);
            return HandleResult(result);
        }

        /// <summary>Nhân bản thành phần lương</summary>
        [HttpPost("{id}/clone")]
        public async Task<IActionResult> Clone(string id)
        {
            var result = await _bl.CloneAsync(id);
            return HandleResult(result);
        }

        /// <summary>Thay đổi trạng thái (active / inactive)</summary>
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> ChangeStatus(string id, [FromBody] ChangeStatusRequest request)
        {
            var result = await _bl.ChangeStatusAsync(id, request.Status);
            return HandleResult(result);
        }

        /// <summary>Thêm thành phần lương từ danh mục hệ thống</summary>
        [HttpPost("from-system/{systemId}")]
        public async Task<IActionResult> AddFromSystem(string systemId)
        {
            var result = await _bl.AddFromSystemAsync(systemId);
            return HandleResult(result);
        }
    }

    public record ChangeStatusRequest(string Status);
}
