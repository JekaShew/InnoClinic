using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories
{
    public interface IBaseRepository<T>
    {
        public void Create(T entity);
        public IQueryable<T> GetAll(bool trackChanges);
        public void Delete(T entity);
        public void Update(T entity);
        public IQueryable<T> GetWithExpression(Expression<Func<T, bool>> expression, bool trackChanges);
    }
}
