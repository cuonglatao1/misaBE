using Dapper;
using MisaBE.Common.DTOs;
using MisaBE.DL.Base;
using System.Text;
using SalaryCompositionSystemEntity = MisaBE.Common.Entities.SalaryCompositionSystem;

namespace MisaBE.DL.SalaryCompositionSystem
{
    public class SalaryCompositionSystemDL : BaseDL<SalaryCompositionSystemEntity>, ISalaryCompositionSystemDL
    {
        public SalaryCompositionSystemDL(string connectionString) : base(connectionString) { }

        protected override string TableName => "pa_salary_composition_system";
        protected override string PrimaryKeyColumn => "SystemId";
        protected override string CodeColumn => "SystemCode";

        public override async Task<PagingResult<SalaryCompositionSystemEntity>> GetPagedAsync(PagingRequest request)
        {
            using var conn = GetConnection();
            var where = new StringBuilder("WHERE IsActive = 1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                where.Append(" AND (SystemCode LIKE @Search OR SystemName LIKE @Search)");
                parameters.Add("Search", $"%{request.SearchText}%");
            }

            var countSql = $"SELECT COUNT(1) FROM pa_salary_composition_system {where}";
            var dataSql = $@"
                SELECT * FROM pa_salary_composition_system
                {where}
                ORDER BY SystemName ASC
                LIMIT @Offset, @PageSize";

            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);

            var total = await conn.ExecuteScalarAsync<int>(countSql, parameters);
            var items = (await conn.QueryAsync<SalaryCompositionSystemEntity>(dataSql, parameters)).ToList();

            return new PagingResult<SalaryCompositionSystemEntity>
            {
                Items = items,
                TotalRecords = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public override async Task<string> InsertAsync(SalaryCompositionSystemEntity entity)
        {
            using var conn = GetConnection();
            entity.SystemId = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;

            const string sql = @"
                INSERT INTO pa_salary_composition_system
                (SystemId, SystemCode, SystemName, ComponentType, Nature, TaxOption,
                 Quota, ValueType, ValueFormula, Description, ShowOnPayslip, IsActive,
                 CreatedDate, ModifiedDate)
                VALUES
                (@SystemId, @SystemCode, @SystemName, @ComponentType, @Nature, @TaxOption,
                 @Quota, @ValueType, @ValueFormula, @Description, @ShowOnPayslip, @IsActive,
                 @CreatedDate, @ModifiedDate)";

            await conn.ExecuteAsync(sql, entity);
            return entity.SystemId;
        }

        public override async Task<int> UpdateAsync(SalaryCompositionSystemEntity entity)
        {
            using var conn = GetConnection();
            entity.ModifiedDate = DateTime.Now;

            const string sql = @"
                UPDATE pa_salary_composition_system SET
                    SystemCode    = @SystemCode,
                    SystemName    = @SystemName,
                    ComponentType = @ComponentType,
                    Nature        = @Nature,
                    TaxOption     = @TaxOption,
                    Quota         = @Quota,
                    ValueType     = @ValueType,
                    ValueFormula  = @ValueFormula,
                    Description   = @Description,
                    ShowOnPayslip = @ShowOnPayslip,
                    IsActive      = @IsActive,
                    ModifiedDate  = @ModifiedDate
                WHERE SystemId = @SystemId";

            return await conn.ExecuteAsync(sql, entity);
        }
    }
}
