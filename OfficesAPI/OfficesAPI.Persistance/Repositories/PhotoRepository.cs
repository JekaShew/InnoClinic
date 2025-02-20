using OfficesAPI.Domain.Data.Models;
using OfficesAPI.Domain.IRepositories;
using System.Linq.Expressions;

namespace OfficesAPI.Persistance.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        public Task<bool> AddPhoto(Photo photo)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeletePhotoById(string photoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Photo>> TakeAllPhotos()
        {
            throw new NotImplementedException();
        }

        public Task<Photo> TakePhotoById(string photoId)
        {
            throw new NotImplementedException();
        }

        public Task<Photo> TakePhotoDTOWithPredicate(Expression<Func<Photo, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePhoto(Photo photo)
        {
            throw new NotImplementedException();
        }
    }
}
