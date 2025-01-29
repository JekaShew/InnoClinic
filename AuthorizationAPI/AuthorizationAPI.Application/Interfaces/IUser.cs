using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Domain.Data.Models;
using InnoShop.CommonLibrary.Response;
using System.Linq.Expressions;

namespace AuthorizationAPI.Application.Interfaces
{
    public interface IUser
    {
        public Task<CustomResponse> AddUser(UserDetailedDTO userDTO);
        public Task<List<UserDTO>> TakeAllUsers();
        public Task<UserDTO> TakeUserById(Guid userId);
        public Task<CustomResponse> UpdateUser(UserDTO userDTO);
        public Task<CustomResponse> DeleteUserById(Guid userId);
        public Task<UserDTO> TakeUserDTOWithPredicate(Expression<Func<User, bool>> predicate);
        public Task<AuthorizationInfoDTO> TakeAuthorizationInfoByEmail(string email);
        public Task<AuthorizationInfoDTO> TakeAuthorizationInfoByUserId(Guid userId);
        public Task<CustomResponse> UpdateAuthorizationInfoOfUser(AuthorizationInfoDTO authorizationInfoDTO);
    }
}
