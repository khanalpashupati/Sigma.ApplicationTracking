using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Sigma.ApplicationTracking.Infrastructure.Data.Context;
using Sigma.ApplicationTracking.Core.Interface;

namespace Sigma.ApplicationTracking.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicantTrackerDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicantTrackerDbContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            try
            {
                IQueryable<T> query = _dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                return await query.ToListAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while getting bulk from the database.", ex);
            }
        }
        public async Task<T> FindAsync(Expression<Func<T, bool>> filter)
        {
            try
            {
                return await _dbSet.FirstOrDefaultAsync(filter);

            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while getting bulk from the database.", ex);
            }
        }

        public async Task<T> GetByIdAsync(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while getting from the database.", ex);
            }
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                var entityEntry = await _dbSet.AddAsync(entity);
                return entityEntry.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while adding to the database.", ex);
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var entityEntry = _dbSet.Update(entity);
                _context.Entry(entity).State = EntityState.Modified;
                return entityEntry.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while updating to the database.", ex);
            }
        }

        public async Task DeleteAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                try
                {
                    _dbSet.Remove(entity);
                }
                catch (DbUpdateException ex)
                {
                    throw new Exception("An error occurred while deleting from the database.", ex);
                }
            }
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
    }
}
