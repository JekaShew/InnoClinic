using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Services.Services
{
    public class UserService : IUserService
    {
        //private readonly IValidator<UserDetailedDTO> _userValidator;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IUserStatusRepository userStatusRepository,
                IRoleRepository roleRepository,
                IRepositoryManager repositoryManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositoryManager = repositoryManager;
        }
        // Add methods ChangeEmailByEmail
        public Guid? TakeCurrentUserId()
        {
           // _userValidator.ValidateAsync();
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                return null;

            var claim = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (claim == null)
                return null;

            return Guid.Parse(claim.Value);
        }

        public async Task<string> GetHashString(string stringToHash)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes($"{stringToHash}");
                var ms = new MemoryStream(inputBytes);
                var hashBytes = await md5.ComputeHashAsync(ms);
                var stringHash = Encoding.UTF8.GetString(hashBytes);
                return stringHash;
            }
        }

        public async Task<UserDetailedDTO> IsCurrentUserAdministrator()
        {      
            var currentUserId = TakeCurrentUserId();
            var adminRole = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(DBConstants.AdministratorRoleId), false))
                    .FirstOrDefault();
            if (adminRole is null || adminRole!.Id.Equals(Guid.Empty))
                return null;

            var currentUser = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId),false))
                    .FirstOrDefault();
            if (!currentUser.RoleId.Equals(adminRole.Id))
                return null;

            return UserMapper.UserToUserDetailedDTO(currentUser)!;
        }

        public async Task<UserDetailedDTO> IsEmailRegistered(string email,bool trackChanges)
        {
            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Email.Equals(email), trackChanges))
                    .FirstOrDefault();
            if (user is null)
                return null;

            return UserMapper.UserToUserDetailedDTO(user)!;
        }

        public async Task<Guid> CreateUserAsync(RegistrationInfoDTO registrationInfoDTO)
        {
            var defaultRole = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(DBConstants.PatientRoleId), false))
                    .FirstOrDefault();
            if (defaultRole is null || defaultRole.Id.Equals(Guid.Empty))
                return Guid.Empty;

            var defaultUserStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(us => us.Id.Equals(DBConstants.ActivatedUserStatusId), false))
                    .FirstOrDefault();
            if (defaultUserStatus is null && defaultUserStatus.Equals(Guid.Empty))
                return Guid.Empty;

            var securityStamp = await GetHashString(registrationInfoDTO.SecretPhrase);
            var secretPhraseHash = await GetHashString($"{registrationInfoDTO.SecretPhrase}{securityStamp}");
            var passwordHash = await GetHashString($"{registrationInfoDTO.Password}{securityStamp}");

            var newUser = UserMapper.RegistrationInfoDTOToUser(registrationInfoDTO);
            newUser.Id = Guid.NewGuid();
            newUser.RoleId = defaultRole.Id;
            newUser.UserStatusId = defaultUserStatus.Id;
            newUser.SecurityStamp = securityStamp;
            newUser.SecretPhraseHash = secretPhraseHash;
            newUser.PasswordHash = passwordHash;

            _repositoryManager.User.CreateUser(newUser);
            await _repositoryManager.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<CommonResponse> DeleteAccountById()
        {
            var deletedUserStatus = (await _repositoryManager
                    .UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Id.Equals(DBConstants.DeletedUserStatusId), false))
                    .FirstOrDefault();
            if(deletedUserStatus is null || deletedUserStatus.Id.Equals(Guid.Empty))
                return new CommonResponse(false, MessageConstants.CheckDBDataMessage);
            
            var currentUserId = TakeCurrentUserId();
            var currentUser = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId), true))
                    .FirstOrDefault();

            currentUser.UserStatusId = deletedUserStatus.Id;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        public async Task<CommonResponse> DeleteUserById(Guid userId)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if(adminUser is null)
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            var userToDelete = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userId), false))
                    .FirstOrDefault();
            _repositoryManager.User.DeleteUser(userToDelete);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessDeleteMessage);
        }

        public async Task<CommonResponse<List<UserInfoDTO>>> TakeAllUsersInfo()
        {
            var users = await _repositoryManager.User.GetAllUsersAsync(false);
            var userInfoDTOs = users.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();

            return new CommonResponse<List<UserInfoDTO>>(true, MessageConstants.SuccessMessage, userInfoDTOs);
        }

        public async Task<CommonResponse<UserDetailedDTO>> GetUserDetailedInfo(Guid userId)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if(adminUser is null)
                return new CommonResponse<UserDetailedDTO>(false, MessageConstants.ForbiddenMessage, null);

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userId),false))
                    .FirstOrDefault();
            var userDetailedInfoDTO = UserMapper.UserToUserDetailedDTO(user);

            return new CommonResponse<UserDetailedDTO>(true, MessageConstants.SuccessMessage, userDetailedInfoDTO);
        }

        public async Task<CommonResponse> UpdateUserInfo(UserInfoDTO userInfoDTO)
        {
            var currentUserId = TakeCurrentUserId();
            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Email.Equals(userInfoDTO.Email), true))
                    .FirstOrDefault();
            if (!user.Id.Equals(currentUserId.Value))
                return new CommonResponse(false, MessageConstants.ForbiddenMessage);

            user = UserMapper.UserInfoDTOToUser(userInfoDTO);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        //Implement
        public Task<CommonResponse> ChangeForgottenPasswordByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Email.Equals(emailSecretPhrasePairDTO.Email), true))
                    .FirstOrDefault();
            if (user is null)
                return new CommonResponse(false, MessageConstants.CheckCredsMessage);

            var enteredSecretPhraseHash = await GetHashString($"{emailSecretPhrasePairDTO.SecretPhrase}{user.SecurityStamp}");

            if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
                return new CommonResponse(false, MessageConstants.CheckCredsMessage);

            user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");

            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        public async Task<CommonResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword)
        {
            var currentUserId = TakeCurrentUserId();

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId),true))
                    .FirstOrDefault();

            var enteredPasswordHash = await GetHashString($"{oldPassword}{user.SecurityStamp}");
            if (!enteredPasswordHash.Equals(user.PasswordHash))
                return new CommonResponse(false, MessageConstants.CheckCredsMessage);

            user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        public async Task<CommonResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPairDTO)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new CommonResponse<UserDetailedDTO>(false, MessageConstants.ForbiddenMessage, null);

            var role = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(userIdRoleIdPairDTO.RoleId),false))
                    .FirstOrDefault();
            if (role is null)
                return new CommonResponse(false, MessageConstants.CheckDBDataMessage);

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userIdRoleIdPairDTO.UserId), true))
                    .FirstOrDefault();
            if (user is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);

            user.RoleId = userIdRoleIdPairDTO.RoleId;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        public async Task<CommonResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new CommonResponse<UserDetailedDTO>(false, MessageConstants.ForbiddenMessage, null);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(us => us.Id.Equals(userIdUserStatusIdPairDTO.UserStatusId), false))
                    .FirstOrDefault();
            if (userStatus is null)
                return new CommonResponse(false, MessageConstants.CheckDBDataMessage);

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userIdUserStatusIdPairDTO.UserId), true))
                    .FirstOrDefault();
            if (user is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);

            user.UserStatusId = userIdUserStatusIdPairDTO.UserStatusId;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }
    }
}
