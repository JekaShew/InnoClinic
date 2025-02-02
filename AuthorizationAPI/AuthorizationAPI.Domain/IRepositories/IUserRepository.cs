using AuthorizationAPI.Domain.Data.Models;
using InnoClinic.CommonLibrary.Response;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories
{
    public interface IUserRepository
    {
        public Task<CustomResponse> AddUser(User user);
        public Task<CustomResponse<List<User>>> TakeAllUsers();
        public Task<CustomResponse<User>> TakeUserById(Guid userId);
        public Task<CustomResponse> UpdateUser(User user);
        public Task<CustomResponse> DeleteUserById(Guid userId);
        public Task<CustomResponse<User>> TakeUserWithPredicate(Expression<Func<User, bool>> predicate);
    }
}
