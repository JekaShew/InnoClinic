using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthorizationAPI.Persistance.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T>  where T : class
    {
        protected AuthDBContext _authDBContext;
        public BaseRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public void Create(T entity) => _authDBContext.Set<T>().Add(entity);

        public void Delete(T entity) => _authDBContext.Set<T>().Remove(entity);

        public void Update(T entity) => _authDBContext.Set<T>().Update(entity);

        public IQueryable<T> GetAll(bool trackChanges)
        {
            return trackChanges ? _authDBContext.Set<T>() : _authDBContext.Set<T>().AsNoTracking();
        }        

        public IQueryable<T> GetWithExpression(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            return trackChanges ? _authDBContext.Set<T>().Where(expression) : _authDBContext.Set<T>().AsNoTracking().Where(expression);
        }
    }
}
