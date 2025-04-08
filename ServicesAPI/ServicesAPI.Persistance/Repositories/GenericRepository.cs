using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseModel
{
    private readonly ServicesDBContext _servicesDBContext;

    public GenericRepository(ServicesDBContext servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public async Task<TEntity?> CreateAsync(TEntity entity)
    {
        await _servicesDBContext.Set<TEntity>().AddAsync(entity);
        
        return entity;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _servicesDBContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> queryMultiple = _servicesDBContext.Set<TEntity>();
        if (includeProperties is not null && includeProperties.Length != 0)
        {
            
            foreach (var includeProperty in includeProperties)
            {
                queryMultiple = queryMultiple.Include(includeProperty);
            }
            
        }    

        return await queryMultiple.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> querySingle = _servicesDBContext.Set<TEntity>();
        if (includeProperties is not null && includeProperties.Length != 0)
        {

            foreach (var includeProperty in includeProperties)
            {
                querySingle = querySingle.Include(includeProperty);
            }

        }

        return await querySingle.FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }

    public async Task<TEntity?> UpdateAsync(Guid Id, TEntity updatedEntity)
    {
        var entityModel = await _servicesDBContext.Set<TEntity>().FindAsync(Id);
        if (entityModel is not null)
        {
            _servicesDBContext.Set<TEntity>().Entry(entityModel).State = EntityState.Detached;
            _servicesDBContext.Set<TEntity>().Update(updatedEntity);
        }

        return updatedEntity;
    }

    public async Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entityModel = await _servicesDBContext.Set<TEntity>().Where(expression).FirstOrDefaultAsync();

        return entityModel;
    }
    public async Task<IEnumerable<TEntity>> GetAllByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entityModels = await _servicesDBContext.Set<TEntity>().Where(expression).ToListAsync();

        return entityModels;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _servicesDBContext.Set<TEntity>();
    }
}
