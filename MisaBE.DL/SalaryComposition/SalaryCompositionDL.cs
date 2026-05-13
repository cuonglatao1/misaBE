using Dapper;
using MisaBE.Common.DTOs;
using MisaBE.DL.Base;
using System.Text;

namespace MisaBE.DL.SalaryComposition
{
    public class SalaryCompositionDL : BaseDL<Common.Entities.SalaryComposition>, ISalaryCompositionDL
    {
        public SalaryCompositionDL(string connectionString) : base(connectionString) { }

        protected override string TableName => "pa_salary_composition";
        protected override string PrimaryKeyColumn => "CompositionId";
        protected override string CodeColumn => "CompositionCode";

        public override async Task<PagingResult<Common.Entities.SalaryComposition>> GetPagedAsync(PagingRequest request)
        {
            using var conn = GetConnection();

            var where = new StringBuilder("WHERE 1=1");
            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                where.Append(" AND (sc.CompositionCode LIKE @Search OR sc.CompositionName LIKE @Search)");
                parameters.Add("Search", $"%{request.SearchText}%");
            }
            if (!string.IsNullOrWhiteSpace(request.OrganizationId))
            {
                where.Append(" AND sc.OrganizationId = @OrgId");
                parameters.Add("OrgId", request.OrganizationId);
            }
            if (!string.IsNullOrWhiteSpace(request.Status))
            {
                where.Append(" AND sc.Status = @Status");
                parameters.Add("Status", request.Status);
            }

            var allowedSort = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "CompositionCode", "CompositionName", "ComponentType", "Nature", "Status", "CreatedDate"
            };
            var sortField = allowedSort.Contains(request.SortField ?? "") ? request.SortField : "CreatedDate";
            var sortDir = request.SortOrder?.ToLower() == "desc" ? "DESC" : "ASC";

            var countSql = $"SELECT COUNT(1) FROM pa_salary_composition sc {where}";
            var dataSql = $@"
                SELECT sc.*, o.OrganizationName
                FROM pa_salary_composition sc
                LEFT JOIN pa_organization o ON sc.OrganizationId = o.OrganizationId
                {where}
                ORDER BY sc.{sortField} {sortDir}
                LIMIT @Offset, @PageSize";

            parameters.Add("Offset", (request.PageNumber - 1) * request.PageSize);
            parameters.Add("PageSize", request.PageSize);

            var total = await conn.ExecuteScalarAsync<int>(countSql, parameters);
            var items = (await conn.QueryAsync<Common.Entities.SalaryComposition>(dataSql, parameters)).ToList();

            return new PagingResult<Common.Entities.SalaryComposition>
            {
                Items = items,
                TotalRecords = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };
        }

        public override async Task<string> InsertAsync(Common.Entities.SalaryComposition entity)
        {
            using var conn = GetConnection();
            entity.CompositionId = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;

            const string sql = @"
                INSERT INTO pa_salary_composition
                (CompositionId, CompositionCode, CompositionName, OrganizationId,
                 ComponentType, Nature, TaxOption, AllowExceedQuota, Quota,
                 ValueType, ValueSource, ValueFormula, Description, ShowOnPayslip,
                 SourceType, SystemId, Status, CreatedDate, ModifiedDate)
                VALUES
                (@CompositionId, @CompositionCode, @CompositionName, @OrganizationId,
                 @ComponentType, @Nature, @TaxOption, @AllowExceedQuota, @Quota,
                 @ValueType, @ValueSource, @ValueFormula, @Description, @ShowOnPayslip,
                 @SourceType, @SystemId, @Status, @CreatedDate, @ModifiedDate)";

            await conn.ExecuteAsync(sql, entity);
            return entity.CompositionId;
        }

        public override async Task<int> UpdateAsync(Common.Entities.SalaryComposition entity)
        {
            using var conn = GetConnection();
            entity.ModifiedDate = DateTime.Now;

            const string sql = @"
                UPDATE pa_salary_composition SET
                    CompositionCode    = @CompositionCode,
                    CompositionName    = @CompositionName,
                    OrganizationId     = @OrganizationId,
                    ComponentType      = @ComponentType,
                    Nature             = @Nature,
                    TaxOption          = @TaxOption,
                    AllowExceedQuota   = @AllowExceedQuota,
                    Quota              = @Quota,
                    ValueType          = @ValueType,
                    ValueSource        = @ValueSource,
                    ValueFormula       = @ValueFormula,
                    Description        = @Description,
                    ShowOnPayslip      = @ShowOnPayslip,
                    SourceType         = @SourceType,
                    Status             = @Status,
                    ModifiedDate       = @ModifiedDate
                WHERE CompositionId = @CompositionId";

            return await conn.ExecuteAsync(sql, entity);
        }

        public async Task<int> UpdateStatusAsync(string id, string status)
        {
            using var conn = GetConnection();
            const string sql = @"
                UPDATE pa_salary_composition
                SET Status = @Status, ModifiedDate = @ModifiedDate
                WHERE CompositionId = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id, Status = status, ModifiedDate = DateTime.Now });
        }
    }
}
