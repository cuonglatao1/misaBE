using Dapper;
using MisaBE.Common.DTOs;
using MisaBE.Common.Entities;
using MisaBE.DL.Base;

namespace MisaBE.DL.Organization
{
    public class OrganizationDL : BaseDL<Organization>, IOrganizationDL
    {
        public OrganizationDL(string connectionString) : base(connectionString) { }

        protected override string TableName => "pa_organization";
        protected override string PrimaryKeyColumn => "OrganizationId";
        protected override string CodeColumn => "OrganizationCode";

        public override async Task<PagingResult<Organization>> GetPagedAsync(PagingRequest request)
        {
            var items = (await GetAllActiveAsync()).ToList();
            return new PagingResult<Organization>
            {
                Items = items,
                TotalRecords = items.Count,
                PageNumber = 1,
                PageSize = items.Count
            };
        }

        public override async Task<string> InsertAsync(Organization entity)
        {
            using var conn = GetConnection();
            entity.OrganizationId = Guid.NewGuid().ToString();
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;

            const string sql = @"
                INSERT INTO pa_organization
                (OrganizationId, OrganizationCode, OrganizationName, ParentId, IsActive, CreatedDate, ModifiedDate)
                VALUES
                (@OrganizationId, @OrganizationCode, @OrganizationName, @ParentId, @IsActive, @CreatedDate, @ModifiedDate)";
            await conn.ExecuteAsync(sql, entity);
            return entity.OrganizationId;
        }

        public override async Task<int> UpdateAsync(Organization entity)
        {
            using var conn = GetConnection();
            entity.ModifiedDate = DateTime.Now;
            const string sql = @"
                UPDATE pa_organization SET
                    OrganizationCode = @OrganizationCode,
                    OrganizationName = @OrganizationName,
                    ParentId         = @ParentId,
                    IsActive         = @IsActive,
                    ModifiedDate     = @ModifiedDate
                WHERE OrganizationId = @OrganizationId";
            return await conn.ExecuteAsync(sql, entity);
        }

        public async Task<IEnumerable<Organization>> GetAllActiveAsync()
        {
            using var conn = GetConnection();
            const string sql = "SELECT * FROM pa_organization WHERE IsActive = 1 ORDER BY OrganizationName";
            return await conn.QueryAsync<Organization>(sql);
        }
    }
}
