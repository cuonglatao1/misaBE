using Microsoft.AspNetCore.Mvc;
using MisaBE.API.Controllers.Base;
using MisaBE.BL.SalaryCompositionSystem;
using MisaBE.Common.DTOs;

namespace MisaBE.API.Controllers
{
    [Route("api/salary-composition-system")]
    public class SalaryCompositionSystemController : BaseController
    {
        private readonly ISalaryCompositionSystemBL _bl;

        public SalaryCompositionSystemController(ISalaryCompositionSystemBL bl) => _bl = bl;

        /// <summary>Danh sách danh mục hệ thống có phân trang và tìm kiếm</summary>
        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] PagingRequest request)
        {
            var result = await _bl.GetPagedAsync(request);
            return Ok(ServiceResult.Ok(result));
        }

        /// <summary>Lấy chi tiết một danh mục hệ thống</summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var entity = await _bl.GetByIdAsync(id);
            if (entity == null) return NotFoundResult();
            return Ok(ServiceResult.Ok(entity));
        }
    }
}
