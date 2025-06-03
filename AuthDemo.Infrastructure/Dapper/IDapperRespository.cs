using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemo.Infrastructure.Dapper
{
    public interface IDapperRepository<T> where T : class
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();

        IEnumerable<T> GetByColumn(string columnName, object value);
        Task<IEnumerable<T>> GetByColumnAsync(string columnName, object value);

        int Insert(T entity);
        Task<int> InsertAsync(T entity);

        int Update(T entity);
        Task<int> UpdateAsync(T entity);

        int Delete(object id);
        Task<int> DeleteAsync(object id);
    }
}
