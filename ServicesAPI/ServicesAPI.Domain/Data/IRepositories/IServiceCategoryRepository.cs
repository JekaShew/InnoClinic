using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface IServiceCategoryRepository : IGenericRepository<ServiceCategory>
{
    public Task<IEnumerable<ServiceCategory>> GetAllWithParametersAsync(ServiceCategoryParameters serviceCategoryParameters);
}
