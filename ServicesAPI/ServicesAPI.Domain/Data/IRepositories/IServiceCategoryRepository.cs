using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using System.Linq.Expressions;

namespace ServicesAPI.Domain.Data.IRepositories;

public interface IServiceCategoryRepository : IGenericRepository<ServiceCategory>
{
    public Task<IEnumerable<ServiceCategory>> GetAllWithParametersAsync(ServiceCategoryParameters? serviceCategoryParameters);
    public new Task<ServiceCategory?> GetByIdAsync(Guid id, params Expression<Func<ServiceCategory, object>>[] includeProperties);
}
