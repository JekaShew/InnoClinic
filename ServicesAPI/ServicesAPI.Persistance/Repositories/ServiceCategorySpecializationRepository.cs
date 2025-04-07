using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Persistance.Repositories;

public class ServiceCategorySpecializationRepository : GenericRepository<ServiceCategorySpecialization>, IServiceCategorySpecializationRepository
{
    private readonly ServicesDBContext _servicesDBContext;
    public ServiceCategorySpecializationRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public async Task<IEnumerable<ServiceCategorySpecialization>> GetAllWithParametersAsync(ServiceCategorySpecializationParameters serviceCategorySpecializationParameters)
    {
        IQueryable<ServiceCategorySpecialization> serviceCategorySpecilaizations = _servicesDBContext.ServiceCategorySpecializations.AsQueryable();

        if (serviceCategorySpecializationParameters.ServiceCategories is not null && serviceCategorySpecializationParameters.ServiceCategories.Count >= 1)
        {
            serviceCategorySpecilaizations = _servicesDBContext.ServiceCategorySpecializations
                .Where(scs => serviceCategorySpecializationParameters.ServiceCategories.Any(sp => sp.Equals(scs.ServiceCategoryId)));
        }

        if (serviceCategorySpecializationParameters.Specializations is not null && serviceCategorySpecializationParameters.Specializations.Count >= 1)
        {
            serviceCategorySpecilaizations = _servicesDBContext.ServiceCategorySpecializations
                .Where(scs => serviceCategorySpecializationParameters.Specializations.Any(sp => sp.Equals(scs.SpecializationId)));
        }

        if (serviceCategorySpecializationParameters.ServiceCategorySearchString is not null
                && serviceCategorySpecializationParameters.ServiceCategorySearchString.Length != 0)
        {
            serviceCategorySpecilaizations = _servicesDBContext.ServiceCategorySpecializations
                .Where(scs =>
                    scs.ServiceCategory.Title
                    .ToLower()
                    .Contains(serviceCategorySpecializationParameters.ServiceCategorySearchString
                        .ToLower()));
        }

        if (serviceCategorySpecializationParameters.SpecializationSearchString is not null
                && serviceCategorySpecializationParameters.SpecializationSearchString.Length != 0)
        {
            serviceCategorySpecilaizations = _servicesDBContext.ServiceCategorySpecializations
                .Where(scs =>
                    scs.Specialization.Title
                    .ToLower()
                    .Contains(serviceCategorySpecializationParameters.SpecializationSearchString
                        .ToLower()));
        }

        IEnumerable<ServiceCategorySpecialization> serviceCategorySpecializationFinalList =
            await serviceCategorySpecilaizations
        .Skip(
            (serviceCategorySpecializationParameters.PageNumber - 1) * serviceCategorySpecializationParameters.PageSize)
        .Take(serviceCategorySpecializationParameters.PageSize)
        .Include(scs => scs.ServiceCategory)
        .Include(scs => scs.Specialization)
        .ToListAsync();

        return serviceCategorySpecializationFinalList;
    }
}
