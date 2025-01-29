using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using InnoShop.CommonLibrary.Response;
using System.Linq.Expressions;

namespace AuthorizationAPI.Application.Interfaces
{
    public interface IUserStatus
    {
        public Task<CustomResponse> AddUserStatus(UserStatusDTO userStatusDTO);
        public Task<List<UserStatusDTO>> TakeAllUserStatuses();
        public Task<UserStatusDTO> TakeUserStatusById(Guid userStatusId);
        public Task<CustomResponse> UpdateUserStatus(UserStatusDTO userStatusDTO);
        public Task<CustomResponse> DeleteUserStatusById(Guid userStatusId);
        public Task<UserStatusDTO> TakeUserStatusDTOWithPredicate(Expression<Func<UserStatus, bool>> predicate);
    }
}
