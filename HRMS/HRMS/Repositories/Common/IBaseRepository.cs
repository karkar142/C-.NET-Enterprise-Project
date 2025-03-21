using System.Linq.Expressions;

namespace HRMS.Repositories.Common
{
    public interface IBaseRepository<T> where T : class
    {
        //CRUD Proccess for
        void Create(T entity);
        IEnumerable<T> GetAll();

        //deleget function for db.context.where
        IEnumerable<T>GetBy(Expression<Func<T, bool>> expression);
        void Update(T entity);
        void Delete(T entity);  
    }
   
}
