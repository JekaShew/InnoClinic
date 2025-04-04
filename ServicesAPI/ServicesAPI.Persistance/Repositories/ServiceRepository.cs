using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class ServiceRepository : GenericRepository<Service>, IServiceReposiotry
{
    public ServiceRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
    }
}
