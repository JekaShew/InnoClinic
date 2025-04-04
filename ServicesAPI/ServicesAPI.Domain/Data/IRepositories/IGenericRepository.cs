using System.Linq.Expressions;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface IGenericRepository<TEntity>
{
    public Task<IEnumerable<TEntity>> GetAllAsync();
    public Task<TEntity?> GetByIdAsync(Guid id);
    public Task<TEntity?> CreateAsync(TEntity entity);
    public Task<TEntity?> UpdateAsync(Guid id, TEntity updatedEntity);
    public Task DeleteAsync(TEntity entity);
    public Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression);
    public Task<IEnumerable<TEntity>> GetAllByExpressionAsync(Expression<Func<TEntity, bool>> expression);
}
