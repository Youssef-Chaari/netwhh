using System.Linq.Expressions;
using BiProject.Api.Data.Oltp;
using BiProject.Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BiProject.Api.Repositories.Implementations
{
    public class OltpRepository<T> : IOltpRepository<T> where T : class
    {
        protected readonly OltpDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public OltpRepository(OltpDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
