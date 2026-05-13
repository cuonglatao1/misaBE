using Microsoft.AspNetCore.Mvc;
using MisaBE.API.Controllers.Base;
using MisaBE.BL.GridConfig;
using MisaBE.Common.Entities;

namespace MisaBE.API.Controllers
{
    [Route("api/grid-configs")]
    public class GridConfigController : BaseController
    {
        private readonly IGridConfigBL _bl;

        public GridConfigController(IGridConfigBL bl) => _bl = bl;

        /// <summary>Lấy cấu hình cột theo gridId và userId</summary>
        [HttpGet("{gridId}")]
        public async Task<IActionResult> GetByGridId(string gridId, [FromQuery] string? userId = null)
        {
            var result = await _bl.GetByGridIdAsync(gridId, userId);
            return HandleResult(result);
        }

        /// <summary>Lưu cấu hình cột (xóa cũ và insert mới trong transaction)</summary>
        [HttpPut("{gridId}")]
        public async Task<IActionResult> SaveConfigs(
            string gridId,
            [FromBody] IEnumerable<GridConfig> configs,
            [FromQuery] string? userId = null)
        {
            var result = await _bl.SaveConfigsAsync(gridId, configs, userId);
            return HandleResult(result);
        }
    }
}
