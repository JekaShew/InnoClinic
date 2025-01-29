using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using AuthorizationAPI.Application.Mappers;
using AuthorizationAPI.Domain.Data;
using AuthorizationAPI.Domain.Data.Models;
using InnoShop.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace AuthorizationAPI.Infrastructure.Repositories
{
    public class UserStatusRepository : IUserStatus
    {
        private readonly AuthDBContext _authDBContext;

        public UserStatusRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public async Task<CustomResponse> AddUserStatus(UserStatusDTO userStatusDTO)
        {        
            var userStatus = UserStatusMapper.UserStatusDTOToUserStatus(userStatusDTO);

            await _authDBContext.UserStatuses.AddAsync(userStatus!);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteUserStatusById(Guid userStatusId)
        {
            var userStatus = await _authDBContext.UserStatuses
                    .AsNoTracking()
                    .FirstOrDefaultAsync(us => us.Id.Equals(userStatusId)); ;

            _authDBContext.UserStatuses.Remove(userStatus);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<List<UserStatusDTO>> TakeAllUserStatuses()
        {
            var userStatusDTOs = await _authDBContext.UserStatuses
                    .AsNoTracking()
                    .Select(us => UserStatusMapper.UserStatusToUserStatusDTO(us))
                    .ToListAsync();

            return userStatusDTOs;         
        }

        public async Task<UserStatusDTO> TakeUserStatusById(Guid userStatusId)
        {
            var userStatusDTO = UserStatusMapper.UserStatusToUserStatusDTO(
            await _authDBContext.UserStatuses
                .AsNoTracking()
                .FirstOrDefaultAsync(us => us.Id == userStatusId));

            return userStatusDTO;
        }

        public async Task<CustomResponse> UpdateUserStatus(UserStatusDTO userStatusDTO)
        {   
            var userStatus = await _authDBContext.UserStatuses.FindAsync(userStatusDTO.Id);

            ApplyPropertiesFromDTOToModel(userStatusDTO, userStatus);

            _authDBContext.UserStatuses.Update(userStatus);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<UserStatusDTO> TakeUserStatusDTOWithPredicate(Expression<Func<UserStatus, bool>> predicate)
        {
            var userStatus = await _authDBContext.UserStatuses
                    .Where(predicate)
                    .FirstOrDefaultAsync();

            if (userStatus is not null)
                return UserStatusMapper.UserStatusToUserStatusDTO(userStatus);

            return null;
        }

        private void ApplyPropertiesFromDTOToModel(UserStatusDTO userStatusDTO, UserStatus userStatus)
        {
            var dtoProperties = userStatusDTO.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProperties = userStatus.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var dtoProperty in dtoProperties)
            {
                var modelProperty = modelProperties.FirstOrDefault(p => p.Name == dtoProperty.Name && p.PropertyType == dtoProperty.PropertyType);
                if (modelProperty != null)
                {
                    modelProperty.SetValue(userStatus, dtoProperty.GetValue(userStatusDTO));
                }
            }
        }
    }
}
