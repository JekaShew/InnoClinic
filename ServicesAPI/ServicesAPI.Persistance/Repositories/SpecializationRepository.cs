using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Persistance.Repositories;

public class SpecializationRepository : GenericRepository<Specialization>, ISpecializationRepository
{
    private readonly ServicesDBContext _servicesDBContext;
    public SpecializationRepository(ServicesDBContext servicesDBContext) : base(servicesDBContext)
    {
        _servicesDBContext = servicesDBContext;
    }

    public async Task<IEnumerable<Specialization>> GetAllWithParametersAsync(SpecializationParameters specializationParameters)
    {
        IQueryable<Specialization> specializations = _servicesDBContext.Specializations.AsQueryable();
        if (specializationParameters.SearchString is not null 
                && specializationParameters.SearchString.Length != 0 )
        {
            specializations = _servicesDBContext.Specializations
                .Where(s => 
                    s.Title
                        .ToLower()
                        .Contains(specializationParameters.SearchString
                            .ToLower()));
        }

        IEnumerable<Specialization> specializationFinalList = 
            await specializations
                .Skip(
                    (specializationParameters.PageNumber - 1) * specializationParameters.PageSize)
                .Take(specializationParameters.PageSize)
                .ToListAsync();

        return specializationFinalList;
    }
}
