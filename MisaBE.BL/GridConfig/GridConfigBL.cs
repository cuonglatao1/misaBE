using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using MisaBE.DL.GridConfig;
using GridConfigEntity = MisaBE.Common.Entities.GridConfig;

namespace MisaBE.BL.GridConfig
{
    public class GridConfigBL : BaseBL<GridConfigEntity>, IGridConfigBL
    {
        private readonly IGridConfigDL _gridConfigDL;

        public GridConfigBL(IGridConfigDL dl) : base(dl)
        {
            _gridConfigDL = dl;
        }

        public async Task<ServiceResult> GetByGridIdAsync(string gridId, string? userId = null)
        {
            var data = await _gridConfigDL.GetByGridIdAsync(gridId, userId);
            return ServiceResult.Ok(data);
        }

        public async Task<ServiceResult> SaveConfigsAsync(
            string gridId, IEnumerable<GridConfigEntity> configs, string? userId = null)
        {
            var count = await _gridConfigDL.SaveConfigsAsync(gridId, configs, userId);
            return ServiceResult.Ok(count, "Lưu cấu hình thành công");
        }
    }
}
