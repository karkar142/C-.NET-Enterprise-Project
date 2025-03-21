using HRMS.DAO;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HRMS.Repositories.Common
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly HRMSApplicationDbContext _dbContext;
        //Generice Dbset for any Entity Models
        private readonly DbSet<T> _dbSet;
        public BaseRepository(HRMSApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet=_dbContext.Set<T>();
        }
        public void Create(T entity)
        {
            _dbContext.Add<T>(entity);
        }

        public void Delete(T entity)
        {
            _dbContext.Update<T>(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.AsNoTracking().AsEnumerable();
        }

        public IEnumerable<T> GetBy(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().Where(expression).AsEnumerable();
        }

        public void Update(T entity)
        {
            _dbContext.Update<T>(entity);
        }
    }
}
