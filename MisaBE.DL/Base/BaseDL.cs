using Dapper;
using MisaBE.Common.DTOs;
using MySqlConnector;

namespace MisaBE.DL.Base
{
    public abstract class BaseDL<T> : IBaseDL<T>
    {
        protected readonly string ConnectionString;

        protected BaseDL(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected MySqlConnection GetConnection() => new(ConnectionString);

        protected abstract string TableName { get; }
        protected abstract string PrimaryKeyColumn { get; }
        protected abstract string CodeColumn { get; }

        public virtual async Task<T?> GetByIdAsync(string id)
        {
            using var conn = GetConnection();
            var sql = $"SELECT * FROM {TableName} WHERE {PrimaryKeyColumn} = @Id";
            return await conn.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
        }

        public virtual async Task<int> DeleteAsync(string id)
        {
            using var conn = GetConnection();
            var sql = $"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} = @Id";
            return await conn.ExecuteAsync(sql, new { Id = id });
        }

        public virtual async Task<int> DeleteMultipleAsync(IEnumerable<string> ids)
        {
            using var conn = GetConnection();
            var idList = ids.ToList();
            if (!idList.Any()) return 0;
            var sql = $"DELETE FROM {TableName} WHERE {PrimaryKeyColumn} IN @Ids";
            return await conn.ExecuteAsync(sql, new { Ids = idList });
        }

        public virtual async Task<bool> IsCodeExistsAsync(string code, string? excludeId = null)
        {
            using var conn = GetConnection();
            string sql;
            if (excludeId == null)
                sql = $"SELECT COUNT(1) FROM {TableName} WHERE {CodeColumn} = @Code";
            else
                sql = $"SELECT COUNT(1) FROM {TableName} WHERE {CodeColumn} = @Code AND {PrimaryKeyColumn} != @ExcludeId";

            var count = await conn.ExecuteScalarAsync<int>(sql, new { Code = code, ExcludeId = excludeId });
            return count > 0;
        }

        public abstract Task<PagingResult<T>> GetPagedAsync(PagingRequest request);
        public abstract Task<string> InsertAsync(T entity);
        public abstract Task<int> UpdateAsync(T entity);
    }
}
