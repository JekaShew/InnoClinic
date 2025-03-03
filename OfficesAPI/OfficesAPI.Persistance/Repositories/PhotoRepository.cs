using MongoDB.Driver;
using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using OfficesAPI.Persistance.Data;

namespace OfficesAPI.Persistance.Repositories;

public class PhotoRepository : IPhotoRepository
{
    private readonly IOfficesContext _officesContext;
    private readonly IMongoCollection<Photo> _photoCollection;
    public PhotoRepository(IOfficesContext officesContext)
    {
        _officesContext = officesContext;
        _photoCollection= _officesContext.GetMongoCollection<Photo>("photos");
    }
    public void AddPhoto(Photo photo)
    {
        _officesContext.AddCommand(() => _photoCollection.InsertOneAsync(photo));
    }

    public void DeletePhotoById(string photoId)
    {
        var filter = Builders<Photo>.Filter.Eq(o => o.Id, photoId);
        _officesContext.AddCommand(() => _photoCollection.DeleteOneAsync(filter));
    }

    public void DeletePhotosOfOfficeByOfficeId(string officeId)
    {
        var filter = Builders<Photo>.Filter.Eq(o => o.OfficeId, officeId);
        _officesContext.AddCommand(() => _photoCollection.DeleteManyAsync(filter));
    }

    public async Task<ICollection<Photo>> GetAllPhotos()
    {
        return await _photoCollection.Find(FilterDefinition<Photo>.Empty).ToListAsync();
    }

    public async Task<ICollection<Photo>> GetPhotoListWithFilter(FilterDefinition<Photo> filter)
    {
        return await _photoCollection.Find(filter).ToListAsync();
    }

    public async Task<Photo> GetPhotoById(string photoId)
    {
        var filter = Builders<Photo>.Filter.Eq(o => o.Id, photoId);
        return await _photoCollection.Find(filter).FirstOrDefaultAsync();
    }

    public void UpdatePhoto(Photo updatedPhoto)
    {
        var filter = Builders<Photo>.Filter.Eq(o => o.Id, updatedPhoto.Id);
        _officesContext.AddCommand(() => _photoCollection.ReplaceOneAsync(filter, updatedPhoto));
    }
}
