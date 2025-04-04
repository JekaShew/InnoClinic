using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

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
}
