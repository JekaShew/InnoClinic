using AuthorizationAPI.Domain.Data.Models;
using InnoClinic.CommonLibrary.Response;
using System.Linq.Expressions;

namespace AuthorizationAPI.Domain.IRepositories
{
    public interface IUserStatusRepository
    {
        public Task<CustomResponse> AddUserStatus(UserStatus userStatus);
        public Task<CustomResponse<List<UserStatus>>> TakeAllUserStatuses();
        public Task<CustomResponse<UserStatus>> TakeUserStatusById(Guid userStatusId);
        public Task<CustomResponse> UpdateUserStatus(UserStatus userStatus);
        public Task<CustomResponse> DeleteUserStatusById(Guid userStatusId);
        public Task<CustomResponse<UserStatus>> TakeUserStatusWithPredicate(Expression<Func<UserStatus, bool>> predicate);
    }
}
