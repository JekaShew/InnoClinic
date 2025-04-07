using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;
using System.Linq.Expressions;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface ISpecializationRepository : IGenericRepository<Specialization>
{
    public Task<IEnumerable<Specialization>> GetAllWithParametersAsync(SpecializationParameters specializationParameters);
    public new Task<Specialization?> GetByIdAsync(Guid id, params Expression<Func<Specialization, object>>[] includeProperties);
}
