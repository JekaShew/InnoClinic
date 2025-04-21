using OfficesAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace OfficesAPI.Domain.IRepositories;

public interface IPhotoRepository
{
    public void AddPhoto(Photo photo);
    public Task<ICollection<Photo>> GetAllPhotos();
    public Task<ICollection<Photo>> GetPhotoListByFilter(Expression<Func<Photo,bool>> expression);
    public Task<Photo> GetPhotoById(Guid photoId);
    public void UpdatePhoto(Photo photo);
    public void DeletePhotoById(Guid photoId);
    public void DeletePhotosOfOfficeByOfficeId(Guid officeId);
}
