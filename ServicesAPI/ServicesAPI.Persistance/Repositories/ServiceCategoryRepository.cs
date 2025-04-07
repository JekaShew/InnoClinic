using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;
using System.Linq.Expressions;

namespace ServicesAPI.Persistance.Repositories;

public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
{
    private readonly ServicesDBContext _servicesDBContext;

    public ServiceCategoryRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public async Task<IEnumerable<ServiceCategory>> GetAllWithParametersAsync(ServiceCategoryParameters serviceCategoryParameters)
    {
        IQueryable<ServiceCategory> serviceCategories = _servicesDBContext.ServiceCategiories.AsQueryable();
        if (serviceCategoryParameters.SearchString is not null
                && serviceCategoryParameters.SearchString.Length != 0)
        {
            serviceCategories = _servicesDBContext.ServiceCategiories
                .Where(s =>
                s.Title
                .ToLower()
                        .Contains(serviceCategoryParameters.SearchString
                            .ToLower()));
        }

        IEnumerable<ServiceCategory> serviceCategoryFinalList =
            await serviceCategories
        .Skip(
        (serviceCategoryParameters.PageNumber - 1) * serviceCategoryParameters.PageSize)
        .Take(serviceCategoryParameters.PageSize)
        .ToListAsync();

        return serviceCategoryFinalList;
    }

    public async new Task<ServiceCategory?> GetByIdAsync(Guid id, params Expression<Func<ServiceCategory, object>>[] includeProperties)
    {
        var serviceCategory = await _servicesDBContext.ServiceCategiories.
                Include(scs => scs.ServiceCategorySpecializations)
                    .ThenInclude(s => s.Specialization)
                .Where(sc => sc.Id.Equals(id))
                .FirstOrDefaultAsync();

        return serviceCategory;
    }
}
