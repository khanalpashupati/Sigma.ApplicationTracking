using System.Linq.Expressions;

namespace Sigma.ApplicationTracking.Core.Interface
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task<T> FindAsync(Expression<Func<T, bool>> filter);
        Task<T> GetByIdAsync(object id);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(object id);

    };
}
