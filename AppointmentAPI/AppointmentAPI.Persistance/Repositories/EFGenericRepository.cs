using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AppointmentAPI.Persistance.Repositories;

public class EFGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseModel
{
    private readonly AppointmentsDBContext _appointmentsDBContext;

    public EFGenericRepository(AppointmentsDBContext appointmentsDBContext)
    {
        _appointmentsDBContext = appointmentsDBContext;
    }

    public async Task<TEntity?> CreateAsync(TEntity entity)
    {
        await _appointmentsDBContext.Set<TEntity>().AddAsync(entity);

        return entity;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _appointmentsDBContext.Set<TEntity>().Remove(entity);

        return Task.CompletedTask;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[]? includeProperties)
    {
        IQueryable<TEntity> queryMultiple = _appointmentsDBContext.Set<TEntity>();
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
        IQueryable<TEntity> querySingle = _appointmentsDBContext.Set<TEntity>();
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
        var entityModel = await _appointmentsDBContext.Set<TEntity>().FindAsync(Id);
        if (entityModel is not null)
        {
            _appointmentsDBContext.Set<TEntity>().Entry(entityModel).State = EntityState.Detached;
            _appointmentsDBContext.Set<TEntity>().Update(updatedEntity);
        }

        return updatedEntity;
    }

    public async Task<TEntity?> GetSingleByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entityModel = await _appointmentsDBContext.Set<TEntity>().Where(expression).FirstOrDefaultAsync();

        return entityModel;
    }
    public async Task<IEnumerable<TEntity>> GetAllByExpressionAsync(Expression<Func<TEntity, bool>> expression)
    {
        var entityModels = await _appointmentsDBContext.Set<TEntity>().Where(expression).ToListAsync();

        return entityModels;
    }

    public IQueryable<TEntity> GetQueryable()
    {
        return _appointmentsDBContext.Set<TEntity>();
    }
}
