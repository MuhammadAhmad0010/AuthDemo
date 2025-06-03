using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Infrastructure.Dapper
{
    public class DapperRepository<T> : IDapperRepository<T> where T : class
    {
        private readonly IDbConnection _connection;

        public DapperRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        private string TableName => typeof(T).Name;

        public T GetById(object id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";
            return _connection.QuerySingleOrDefault<T>(sql, new { Id = id })!;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var sql = $"SELECT * FROM {TableName} WHERE Id = @Id";
            return (await _connection.QuerySingleOrDefaultAsync<T>(sql, new { Id = id }))!;
        }

        public IEnumerable<T> GetAll()
        {
            var sql = $"SELECT * FROM {TableName}";
            return _connection.Query<T>(sql);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var sql = $"SELECT * FROM {TableName}";
            return await _connection.QueryAsync<T>(sql);
        }

        public int Insert(T entity)
        {
            var insertQuery = GenerateInsertQuery();
            var parameters = GetScalarProperties(entity);
            return _connection.Execute(insertQuery, parameters);
        }

        public async Task<int> InsertAsync(T entity)
        {
            var insertQuery = GenerateInsertQuery();
            var parameters = GetScalarProperties(entity);
            return await _connection.ExecuteAsync(insertQuery, parameters);
        }

        public int Update(T entity)
        {
            var updateQuery = GenerateUpdateQuery();
            var parameters = GetScalarProperties(entity);
            return _connection.Execute(updateQuery, parameters);
        }

        public async Task<int> UpdateAsync(T entity)
        {
            var updateQuery = GenerateUpdateQuery();
            var parameters = GetScalarProperties(entity);
            return await _connection.ExecuteAsync(updateQuery, parameters);
        }

        public int Delete(object id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @Id";
            return _connection.Execute(sql, new { Id = id });
        }

        public async Task<int> DeleteAsync(object id)
        {
            var sql = $"DELETE FROM {TableName} WHERE Id = @Id";
            return await _connection.ExecuteAsync(sql, new { Id = id });
        }

        public IEnumerable<T> GetByColumn(string columnName, object value)
        {
            if (!IsValidColumn(columnName))
                throw new ArgumentException($"Invalid column name: {columnName}");

            var sql = $"SELECT * FROM {TableName} WHERE {columnName} = @Value";
            return _connection.Query<T>(sql, new { Value = value });
        }

        public async Task<IEnumerable<T>> GetByColumnAsync(string columnName, object value)
        {
            if (!IsValidColumn(columnName))
                throw new ArgumentException($"Invalid column name: {columnName}");

            var sql = $"SELECT * FROM {TableName} WHERE {columnName} = @Value";
            return await _connection.QueryAsync<T>(sql, new { Value = value });
        }

        private string GenerateInsertQuery()
        {
            var insertQuery = new StringBuilder($"INSERT INTO {TableName} (");

            var properties = GetScalarPropertyInfos().Where(p => p.Name != "Id");

            foreach (var prop in properties)
                insertQuery.Append($"{prop.Name},");

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(") VALUES (");

            foreach (var prop in properties)
                insertQuery.Append($"@{prop.Name},");

            insertQuery
                .Remove(insertQuery.Length - 1, 1)
                .Append(")");

            return insertQuery.ToString();
        }

        private string GenerateUpdateQuery()
        {
            var updateQuery = new StringBuilder($"UPDATE {TableName} SET ");

            var properties = GetScalarPropertyInfos().Where(p => p.Name != "Id");

            foreach (var prop in properties)
                updateQuery.Append($"{prop.Name} = @{prop.Name},");

            updateQuery
                .Remove(updateQuery.Length - 1, 1)
                .Append(" WHERE Id = @Id");

            return updateQuery.ToString();
        }

        private bool IsValidColumn(string columnName)
        {
            return GetScalarPropertyInfos().Any(p => p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));
        }

        private static IEnumerable<PropertyInfo> GetScalarPropertyInfos()
        {
            return typeof(T).GetProperties()
                .Where(p => IsSimpleType(p.PropertyType));
        }

        private static object GetScalarProperties(T entity)
        {
            var props = typeof(T).GetProperties()
                .Where(p => IsSimpleType(p.PropertyType))
                .ToDictionary(p => p.Name, p => p.GetValue(entity));

            return props;
        }

        private static bool IsSimpleType(Type type)
        {
            var underlying = Nullable.GetUnderlyingType(type) ?? type;
            return underlying.IsPrimitive
                   || underlying.IsEnum
                   || underlying == typeof(string)
                   || underlying == typeof(decimal)
                   || underlying == typeof(DateTime)
                   || underlying == typeof(Guid)
                   || underlying == typeof(DateTimeOffset);
        }
    }
}
