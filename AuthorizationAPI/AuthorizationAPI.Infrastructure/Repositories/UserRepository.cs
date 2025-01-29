using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using AuthorizationAPI.Application.Mappers;
using AuthorizationAPI.Domain.Data;
using AuthorizationAPI.Domain.Data.Models;
using Azure;
using InnoShop.CommonLibrary.Logs;
using InnoShop.CommonLibrary.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace AuthorizationAPI.Infrastructure.Repositories
{
    public class UserRepository : IUser
    {
        private readonly AuthDBContext _authDBContext;

        public UserRepository(AuthDBContext authDBContext)
        {
            _authDBContext = authDBContext;
        }

        public async Task<CustomResponse> AddUser(UserDetailedDTO userDetailedDTO)
        {
           
            var roleId = await _authDBContext.Roles
                    .AsNoTracking()
                    .Where(r => r.Title == "Patient")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

            var userStatusId = await _authDBContext.UserStatuses
                    .AsNoTracking()
                    .Where(us => us.Title == "Activated")
                    .Select(r => r.Id)
                    .FirstOrDefaultAsync();

            if (roleId == null || roleId == Guid.Empty)
                return new CustomResponse(false, "There is no Default Role named Patient in DB!");

            userDetailedDTO.RoleId = roleId;

            if (userStatusId == null && userStatusId == Guid.Empty)
                return new CustomResponse(false, "There is no Default User Status named Acitvated in DB!");

            userDetailedDTO.UserStatusId = userStatusId;

            var newUser = UserMapper.UserDetailedDTOToUser(userDetailedDTO);

            await _authDBContext.Users.AddAsync(newUser);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Added!");
        }

        public async Task<CustomResponse> DeleteUserById(Guid userId)
        { 
            var user = await _authDBContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

            _authDBContext.Users.Remove(user);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Deleted!");
        }

        public async Task<List<UserDTO>> TakeAllUsers()
        {
            var userDTOs = await _authDBContext.Users
                    .AsNoTracking()
                    .Select(u => UserMapper.UserToUserDTO(u))
                    .ToListAsync();

            return userDTOs;          
        }

        public async Task<UserDTO> TakeUserById(Guid userId)
        {
            var user = await _authDBContext.Users
                    .Include(r => r.Role)
                    .Include(us => us.UserStatus)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            var userDTO = UserMapper.UserToUserDTO(user);

            return userDTO;
        }

        public async Task<CustomResponse> UpdateUser(UserDTO userDTO)
        {
            var user = await _authDBContext.Users.FindAsync(userDTO.Id); 

            ApplyPropertiesFromDTOToModel(userDTO, user);

            _authDBContext.Users.Update(user);
            await _authDBContext.SaveChangesAsync();

            return new CustomResponse(true, "Successfully Updated!");
        }

        public async Task<AuthorizationInfoDTO> TakeAuthorizationInfoByUserId(Guid userId)
        {
            var user = await _authDBContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id.Equals(userId));

            if (user is null)
                return null;
            return UserMapper.UserToAuthorizationInfoDTO(user);            
        }

        public async Task<AuthorizationInfoDTO> TakeAuthorizationInfoByEmail(string email)
        {
            var user = await _authDBContext.Users
                   .AsNoTracking()
                   .FirstOrDefaultAsync(u => u.Email.Equals(email));

            if (user is null)
                return null;
            return UserMapper.UserToAuthorizationInfoDTO(user);
        }

        public async Task<CustomResponse> UpdateAuthorizationInfoOfUser(AuthorizationInfoDTO authorizationInfoDTO)
        {
           
                var user = await _authDBContext.Users
                    .FindAsync(authorizationInfoDTO.Id);

                if (user is null)
                    return new CustomResponse(false, "User not found!");
                
                user.PasswordHash = authorizationInfoDTO.PasswordHash;
                user.SecretPhraseHash = authorizationInfoDTO.SecretPhraseHash;
                user.SecurityStamp = authorizationInfoDTO.SecurityStamp;

                _authDBContext.Users.Update(user);
                await _authDBContext.SaveChangesAsync();
                return new CustomResponse(true, "Successfylly Updated!");
        }

        public async Task<UserDTO> TakeUserDTOWithPredicate(Expression<Func<User, bool>> predicate)
        {
            var user = await _authDBContext.Users
                    .Where(predicate)
                    .FirstOrDefaultAsync();

            if (user != null)
                return UserMapper.UserToUserDTO(user);

            return null;
        }

        private void ApplyPropertiesFromDTOToModel(UserDTO userDTO, User user)
        {
            var dtoProperties = userDTO.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProperties = user.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var dtoProperty in dtoProperties)
            {
                var modelProperty = modelProperties.FirstOrDefault(p => p.Name == dtoProperty.Name && p.PropertyType == dtoProperty.PropertyType);
                if (modelProperty != null)
                {
                    modelProperty.SetValue(user, dtoProperty.GetValue(userDTO));
                }
            }
        }

    }
}
