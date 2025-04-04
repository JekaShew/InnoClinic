using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Shared.DTOs.ServiceDTOs;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Persistance.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceReposiotry
{
    private readonly ServicesDBContext _servicesDBContext;

    public ServiceRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public async Task<IEnumerable<Service>> GetAllWithParametersAsync(ServiceParameters serviceParameters)
    {
        IQueryable<Service> services = _servicesDBContext.Services.AsQueryable();

        if (serviceParameters.MaxPrice is not null
                && serviceParameters.MaxPrice >= serviceParameters.MinPrice)
        {
            services.Where(s =>
                s.Price <= serviceParameters.MaxPrice);
        }

        if (serviceParameters.MinPrice is not null
               && serviceParameters.MinPrice <= serviceParameters.MaxPrice)
        {
            services.Where(s =>
                s.Price >= serviceParameters.MinPrice);
        }

        if (serviceParameters.ServiceCategories.Count >= 1)
        {
            services = _servicesDBContext.Services
                .Where(s => serviceParameters.ServiceCategories.Any(sp => sp.Equals(s.ServiceCategoryId)));
        }

        if (serviceParameters.SearchString is not null
                && serviceParameters.SearchString.Length != 0)
        {
            services = _servicesDBContext.Services
                .Where(s =>
                s.Title
                .ToLower()
                        .Contains(serviceParameters.SearchString
                            .ToLower()));
        }

        IEnumerable<Service> serviceFinalList =
            await services
        .Skip(
        (serviceParameters.PageNumber - 1) * serviceParameters.PageSize)
        .Take(serviceParameters.PageSize)
                .ToListAsync();

        return serviceFinalList;
    }
}
