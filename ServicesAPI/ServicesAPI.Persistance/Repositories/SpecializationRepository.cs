using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;
using System.Linq.Expressions;

namespace ServicesAPI.Persistance.Repositories;

public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
{
    private readonly ServicesDBContext _servicesDBContext;
    public SpecializationRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public async Task<IEnumerable<Specialization>> GetAllWithParametersAsync(SpecializationParameters? specializationParameters)
    {
        IQueryable<Specialization> specializations = _servicesDBContext.Specializations.AsQueryable();
        if(specializationParameters is null)
        {
            specializationParameters = new SpecializationParameters();
        }

        if (specializationParameters.SearchString is not null 
                && specializationParameters.SearchString.Length != 0 )
        {
            specializations = _servicesDBContext.Specializations
                .Where(s => 
                    s.Title
                        .ToLower()
                        .Contains(specializationParameters.SearchString
                            .ToLower()));
        }

        IEnumerable<Specialization> specializationFinalList = 
            await specializations
                .Skip(
                    (specializationParameters.PageNumber - 1) * specializationParameters.PageSize)
                .Take(specializationParameters.PageSize)
                .ToListAsync();

        return specializationFinalList;
    }

    public async new Task<Specialization?> GetByIdAsync(Guid id, params Expression<Func<ServiceCategory, object>>[] includeProperties)
    {
        var specialization = await _servicesDBContext.Specializations.
                Include(scs => scs.ServiceCategorySpecializations)
                    .ThenInclude(sc => sc.ServiceCategory)
                .Where(s => s.Id.Equals(id))
                .FirstOrDefaultAsync();

        return specialization;
    }
}
