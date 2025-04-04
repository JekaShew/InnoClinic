using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
{
    public ServiceCategoryRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
    }
}
