using System.Linq.Expressions;

namespace AppointmentAPI.Domain.IRepositories;


public interface IGenericRepository<TEntity>
{
    public Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
    public Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[] includeProperties);
    public IQueryable<TEntity> GetQueryable();
    public Task<TEntity?> CreateAsync(TEntity entity);
    public Task<TEntity?> UpdateAsync(Guid id, TEntity updatedEntity);
    public Task DeleteAsync(TEntity entity);
    public Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression);
    public Task<IEnumerable<TEntity>> GetAllByExpressionAsync(Expression<Func<TEntity, bool>> expression);
}