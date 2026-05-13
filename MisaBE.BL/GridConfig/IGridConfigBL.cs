using MisaBE.BL.Base;
using MisaBE.Common.DTOs;
using MisaBE.Common.Entities;

namespace MisaBE.BL.GridConfig
{
    public interface IGridConfigBL : IBaseBL<GridConfig>
    {
        Task<ServiceResult> GetByGridIdAsync(string gridId, string? userId = null);
        Task<ServiceResult> SaveConfigsAsync(string gridId, IEnumerable<GridConfig> configs, string? userId = null);
    }
}
