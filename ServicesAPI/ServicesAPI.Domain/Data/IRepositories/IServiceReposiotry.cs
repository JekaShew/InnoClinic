using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface IServiceReposiotry : IGenericRepository<Service>
{
    public Task<IEnumerable<Service>> GetAllWithParametersAsync(ServiceParameters? serviceParameters);
}
