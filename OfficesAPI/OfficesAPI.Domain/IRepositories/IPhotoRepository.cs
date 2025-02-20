using OfficesAPI.Domain.Data.Models;

namespace OfficesAPI.Domain.IRepositories;

public interface IPhotoRepository
{
    public void AddPhoto(Photo photo);
    public Task<List<Photo>> TakeAllPhotos();
    public Task<Photo> TakePhotoById(string photoId);
    public void UpdatePhoto(Photo photo);
    public void DeletePhotoById(string photoId);
}
