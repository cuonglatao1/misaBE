using MisaBE.DL.Base;
using GridConfigEntity = MisaBE.Common.Entities.GridConfig;

namespace MisaBE.DL.GridConfig
{
    public interface IGridConfigDL : IBaseDL<GridConfigEntity>
    {
        Task<IEnumerable<GridConfigEntity>> GetByGridIdAsync(string gridId, string? userId = null);
        Task<int> SaveConfigsAsync(string gridId, IEnumerable<GridConfigEntity> configs, string? userId = null);
    }
}
