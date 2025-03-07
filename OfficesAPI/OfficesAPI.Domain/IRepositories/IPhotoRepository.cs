using MongoDB.Driver;
using OfficesAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace OfficesAPI.Domain.IRepositories;

public interface IPhotoRepository
{
    public void AddPhoto(Photo photo);
    public Task<ICollection<Photo>> GetAllPhotos();
    public Task<ICollection<Photo>> GetPhotoListByFilter(Expression<Func<Photo,bool>> expression);
    public Task<Photo> GetPhotoById(string photoId);
    public void UpdatePhoto(Photo photo);
    public void DeletePhotoById(string photoId);
    public void DeletePhotosOfOfficeByOfficeId(string officeId);
}
