using OfficesAPI.Domain.Data.Models;
using System.Linq.Expressions;

namespace OfficesAPI.Domain.IRepositories
{
    public interface IPhotoRepository
    {
        public Task<bool> AddPhoto(Photo photo);
        public Task<List<Photo>> TakeAllPhotos();
        public Task<Photo> TakePhotoById(string photoId);
        public Task<bool> UpdatePhoto(Photo photo);
        public Task<bool> DeletePhotoById(string photoId);
        public Task<Photo> TakePhotoDTOWithPredicate(Expression<Func<Photo, bool>> predicate);
    }
}
