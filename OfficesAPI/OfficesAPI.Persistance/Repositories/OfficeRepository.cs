using MongoDB.Driver;
using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Data;

namespace OfficesAPI.Persistance.Repositories;

public class OfficeRepository : IOfficeRepository
{
    private readonly IOfficesContext _officesContext;
    private readonly IMongoCollection<Office> _officeCollection;
    public OfficeRepository(IOfficesContext officeContext)
    {
        _officesContext = officeContext;
        _officeCollection = _officesContext.GetMongoCollection<Office>("offices"); 
    }
    public void CreateOffice(Office office)
    {
        _officesContext.AddCommand(() => _officeCollection.InsertOneAsync(office));
    }

    public void DeleteOfficeById(string officeId)
    {
        var filter = Builders<Office>.Filter.Eq(o => o.Id, officeId);
        _officesContext.AddCommand(() => _officeCollection.DeleteOneAsync(filter));
    }

    public async Task<ICollection<Office>> GetAllOfficesAsync()
    {
        return await _officeCollection.Find(FilterDefinition<Office>.Empty).ToListAsync();
    }

    public async Task<Office> GetOfficeByIdAsync(string officeId)
    {
        var filter = Builders<Office>.Filter.Eq(o => o.Id, officeId);
        return await _officeCollection.Find(filter).FirstOrDefaultAsync();
    }

    public void UpdateOffice(Office updatedoffice)
    {
        var filter = Builders<Office>.Filter.Eq(o => o.Id, updatedoffice.Id);
        _officesContext.AddCommand(() => _officeCollection.ReplaceOneAsync(filter, updatedoffice));
    }
}
