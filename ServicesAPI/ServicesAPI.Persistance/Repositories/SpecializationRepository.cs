using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;

namespace ServicesAPI.Persistance.Repositories;

public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
{
    public SpecializationRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
    }
}
