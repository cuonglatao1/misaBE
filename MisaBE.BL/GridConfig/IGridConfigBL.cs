using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using GridConfigEntity = MisaBE.Common.Entities.GridConfig;

namespace MisaBE.BL.GridConfig
{
    public interface IGridConfigBL : IBaseBL<GridConfigEntity>
    {
        Task<ServiceResult> GetByGridIdAsync(string gridId, string? userId = null);
        Task<ServiceResult> SaveConfigsAsync(string gridId, IEnumerable<GridConfigEntity> configs, string? userId = null);
    }
}
