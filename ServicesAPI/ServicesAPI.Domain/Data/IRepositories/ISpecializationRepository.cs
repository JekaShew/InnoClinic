using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface ISpecializationRepository : IGenericRepository<Specialization>
{
    public Task<IEnumerable<Specialization>> GetAllWithParametersAsync(SpecializationParameters specializationParameters);
}
