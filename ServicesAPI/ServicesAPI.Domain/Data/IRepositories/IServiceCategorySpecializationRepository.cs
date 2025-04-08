using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface IServiceCategorySpecializationRepository : IGenericRepository<ServiceCategorySpecialization>
{
    public Task<IEnumerable<ServiceCategorySpecialization>> GetAllWithParametersAsync(ServiceCategorySpecializationParameters? serviceParameters);
}
