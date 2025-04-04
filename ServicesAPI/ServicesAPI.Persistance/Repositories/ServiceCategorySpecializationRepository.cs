using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class ServiceCategorySpecializationRepository : GenericRepository<ServiceCategorySpecialization>, IServiceCategorySpecializationRepository
{
    public ServiceCategorySpecializationRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
    }
}
