using Dapper;
using MisaBE.Common.DTOs;
using MisaBE.DL.Base;
using GridConfigEntity = MisaBE.Common.Entities.GridConfig;

namespace MisaBE.DL.GridConfig
{
    public class GridConfigDL : BaseDL<GridConfigEntity>, IGridConfigDL
    {
        public GridConfigDL(string connectionString) : base(connectionString) { }

        protected override string TableName => "pa_grid_config";
        protected override string PrimaryKeyColumn => "ConfigId";
        protected override string CodeColumn => "ColumnField";

        public override async Task<PagingResult<GridConfigEntity>> GetPagedAsync(PagingRequest request)
        {
            using var conn = GetConnection();
            var items = (await conn.QueryAsync<GridConfigEntity>("SELECT * FROM pa_grid_config ORDER BY SortOrder")).ToList();
            return new PagingResult<GridConfigEntity>
            {
                Items = items,
                TotalRecords = items.Count,
                PageNumber = 1,
                PageSize = items.Count
            };
        }

        public override async Task<string> InsertAsync(GridConfigEntity entity)
        {
            using var conn = GetConnection();
            entity.ConfigId = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;

            const string sql = @"
                INSERT INTO pa_grid_config
                (ConfigId, GridId, ColumnField, ColumnCaption, ColumnWidth,
                 IsVisible, IsFixed, FixedPosition, SortOrder, UserId, CreatedDate, ModifiedDate)
                VALUES
                (@ConfigId, @GridId, @ColumnField, @ColumnCaption, @ColumnWidth,
                 @IsVisible, @IsFixed, @FixedPosition, @SortOrder, @UserId, @CreatedDate, @ModifiedDate)
                ON DUPLICATE KEY UPDATE
                    ColumnCaption = VALUES(ColumnCaption),
                    ColumnWidth   = VALUES(ColumnWidth),
                    IsVisible     = VALUES(IsVisible),
                    IsFixed       = VALUES(IsFixed),
                    FixedPosition = VALUES(FixedPosition),
                    SortOrder     = VALUES(SortOrder),
                    ModifiedDate  = VALUES(ModifiedDate)";
            await conn.ExecuteAsync(sql, entity);
            return entity.ConfigId;
        }

        public override async Task<int> UpdateAsync(GridConfigEntity entity)
        {
            using var conn = GetConnection();
            entity.ModifiedDate = DateTime.Now;
            const string sql = @"
                UPDATE pa_grid_config SET
                    ColumnCaption = @ColumnCaption,
                    ColumnWidth   = @ColumnWidth,
                    IsVisible     = @IsVisible,
                    IsFixed       = @IsFixed,
                    FixedPosition = @FixedPosition,
                    SortOrder     = @SortOrder,
                    ModifiedDate  = @ModifiedDate
                WHERE ConfigId = @ConfigId";
            return await conn.ExecuteAsync(sql, entity);
        }

        public async Task<IEnumerable<GridConfigEntity>> GetByGridIdAsync(string gridId, string? userId = null)
        {
            using var conn = GetConnection();
            const string sql = @"
                SELECT * FROM pa_grid_config
                WHERE GridId = @GridId AND (UserId = @UserId OR UserId IS NULL)
                ORDER BY SortOrder";
            return await conn.QueryAsync<GridConfigEntity>(sql, new { GridId = gridId, UserId = userId });
        }

        public async Task<int> SaveConfigsAsync(string gridId, IEnumerable<GridConfigEntity> configs, string? userId = null)
        {
            using var conn = GetConnection();
            await conn.OpenAsync();
            using var tx = await conn.BeginTransactionAsync();
            try
            {
                await conn.ExecuteAsync(
                    "DELETE FROM pa_grid_config WHERE GridId = @GridId AND (UserId = @UserId OR UserId IS NULL)",
                    new { GridId = gridId, UserId = userId }, tx);

                int order = 0;
                foreach (var cfg in configs)
                {
                    cfg.ConfigId     = Guid.NewGuid().ToString();
                    cfg.GridId       = gridId;
                    cfg.UserId       = userId;
                    cfg.SortOrder    = order++;
                    cfg.CreatedDate  = DateTime.Now;
                    cfg.ModifiedDate = DateTime.Now;

                    await conn.ExecuteAsync(@"
                        INSERT INTO pa_grid_config
                        (ConfigId, GridId, ColumnField, ColumnCaption, ColumnWidth,
                         IsVisible, IsFixed, FixedPosition, SortOrder, UserId, CreatedDate, ModifiedDate)
                        VALUES
                        (@ConfigId, @GridId, @ColumnField, @ColumnCaption, @ColumnWidth,
                         @IsVisible, @IsFixed, @FixedPosition, @SortOrder, @UserId, @CreatedDate, @ModifiedDate)",
                        cfg, tx);
                }

                await tx.CommitAsync();
                return order;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
    }
}
