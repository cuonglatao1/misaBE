using MisaBE.Common.Entities;
using MisaBE.DL.Base;

namespace MisaBE.DL.GridConfig
{
    public interface IGridConfigDL : IBaseDL<GridConfig>
    {
        Task<IEnumerable<GridConfig>> GetByGridIdAsync(string gridId, string? userId = null);
        Task<int> SaveConfigsAsync(string gridId, IEnumerable<GridConfig> configs, string? userId = null);
    }
}
