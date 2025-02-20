using OfficesAPI.Domain.Data.Models;

namespace OfficesAPI.Domain.IRepositories;

public interface IOfficeRepository
{
    public void CreateOffice(Office office);
    public Task<List<Office>> GetAllOfficesAsync();
    public Task<Office> GetOfficeByIdAsync(string officeId);
    public void UpdateOffice(Office office);
    public void DeleteOfficeById(string officeId);
}
