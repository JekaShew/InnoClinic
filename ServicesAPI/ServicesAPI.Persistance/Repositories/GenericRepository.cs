using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
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

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _servicesDBContext.Set<TEntity>().ToListAsync();
    }

   

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await _servicesDBContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity?> UpdateAsync(Guid Id, TEntity updatedEntity)
    {
        _servicesDBContext.Set<TEntity>().Update(updatedEntity);

        var serviceCategorySpecialization = await _servicesDBContext.Set<TEntity>().FindAsync(Id);
        if (updatedEntity is not null)
        {
            _servicesDBContext.Set<TEntity>().Entry(updatedEntity).State = EntityState.Detached;
            _servicesDBContext.Set<TEntity>().Update(updatedEntity);
        }

        return updatedEntity;
    }

    public Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }
    public Task<IEnumerable<TEntity>> GetAllByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        throw new NotImplementedException();
    }
}
