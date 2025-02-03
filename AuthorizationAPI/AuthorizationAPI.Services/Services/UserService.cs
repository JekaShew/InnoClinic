using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.DTOs;
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
        private readonly IValidator<UserDetailedDTO> _userValidator;
        private readonly IRepositoryManager _repositoryManager;
        //private readonly IUserRepository _userRepository;
        //private readonly IRoleRepository _roleRepository;
        //private readonly IUserStatusRepository _userStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(
                IHttpContextAccessor httpContextAccessor,
                IUserRepository userRepository,
                IUserStatusRepository userStatusRepository,
                IRoleRepository roleRepository,
                IRepositoryManager repositoryManager)
        {
            _httpContextAccessor = httpContextAccessor;
            //_userRepository = userRepository;
            //_userStatusRepository = userStatusRepository;
            //_roleRepository = roleRepository;
            _repositoryManager = repositoryManager;
        }
        // Add Common methods ChangeEmail ActivateEmail-> default UserStatus change to "Not Activated"
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

        public async Task<bool> IsCurrentUserAdministrator()
        {      
            var currentUserId = TakeCurrentUserId();
            var adminRole = (await _repositoryManager.Role.GetRolesWithExpressionAsync(r => r.Title == "Administrator", false)).FirstOrDefault();
            

            if (adminRole is null || adminRole!.Id.Equals(Guid.Empty))
                return false;

            var currentUser = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId),false)).FirstOrDefault();
            if (!currentUser.RoleId.Equals(adminRole.Id))
                return false;
            return true;
        }

        public async Task<CommonResponse> CreateUser(RegistrationInfoDTO registrationInfoDTO)
        {
            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Email.Equals(registrationInfoDTO.Email), false)).FirstOrDefault();
            if (user is not null)
                return new CommonResponse(false, $"Email {registrationInfoDTO.Email} has been already registered!");

            var defaultRole = (await _repositoryManager.Role.GetRolesWithExpressionAsync(r => r.Title == "Patient", false)).FirstOrDefault();
            if (defaultRole is null || defaultRole.Id.Equals(Guid.Empty))
                return new CommonResponse(false, "There is no Default Role named Patient in DB!");

            var defaultUserStatus = (await _repositoryManager.UserStatus.GetUserStatusesWithExpressionAsync(us => us.Title == "Activated", false)).FirstOrDefault();
            if (defaultUserStatus is null && defaultUserStatus.Equals(Guid.Empty))
                return new CommonResponse(false, "There is no Default User Status named Acitvated in DB!");

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

            return new CommonResponse(true, "Successfully Created!");
        }

        public async Task<CommonResponse> DeleteAccountById()
        {
            var deletedUserStatus = (await _repositoryManager
                    .UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Title == "Deleted", false))
                    .FirstOrDefault();
            if(deletedUserStatus is null || deletedUserStatus.Id.Equals(Guid.Empty))
                return new CommonResponse(false, "There is no Default User Status named Deleted in DB!");
            
            var currentUserId = TakeCurrentUserId();
            var currentUser = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId), true)).FirstOrDefault();

            currentUser.UserStatusId = deletedUserStatus.Id;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "User Status was changed to Deleted!");
        }

        public async Task<CommonResponse> DeleteUserById(Guid userId)
        {
            var isAdmin = await IsCurrentUserAdministrator();
            if(isAdmin == false)
                return new CommonResponse(false, "Forbidden Action!");

            var userToDelete = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(userId), false)).FirstOrDefault();
            _repositoryManager.User.DeleteUser(userToDelete);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "Successfully Deleted!");
        }

        public async Task<CommonResponse<List<UserInfoDTO>>> TakeAllUsersInfo()
        {
            var users = await _repositoryManager.User.GetAllUsersAsync(false);
            var userInfoDTOs = users.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();
            
            return new CommonResponse<List<UserInfoDTO>>(true,"Success!", userInfoDTOs);
        }

        public async Task<CommonResponse<UserDetailedDTO>> GetUserDetailedInfo(Guid userId)
        {
            var isAdmin = await IsCurrentUserAdministrator();
            if(isAdmin == false)
                return new CommonResponse<UserDetailedDTO>(false, "Forbidden Action!",null);

            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(userId),false)).FirstOrDefault();
            var userDetailedInfoDTO = UserMapper.UserToUserDetailedDTO(user);

            return new CommonResponse<UserDetailedDTO>(true, "Success!", userDetailedInfoDTO);
        }

        public async Task<CommonResponse> UpdateUserInfo(UserInfoDTO userInfoDTO)
        {
            var currentUserId = TakeCurrentUserId();
            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Email.Equals(userInfoDTO.Email), true)).FirstOrDefault();
            if (!user.Id.Equals(currentUserId.Value))
                return new CommonResponse(false, "Forbidden Action!");

            user = UserMapper.UserInfoDTOToUser(userInfoDTO);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "Success!");
        }

        //Implement
        public Task<CommonResponse> ChangeForgottenPasswordByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Email.Equals(emailSecretPhrasePairDTO.Email), true)).FirstOrDefault();
            if (user is null)
                return new CommonResponse(false, "User not Found!");

            var enteredSecretPhraseHash = await GetHashString($"{emailSecretPhrasePairDTO.SecretPhrase}{user.SecurityStamp}");

            if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
                return new CommonResponse(false, "Check credetials you have entered! Wrong Email or Secret Phrase!");

            user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");

            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "Success!");
        }

        public async Task<CommonResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword)
        {
            var currentUserId = TakeCurrentUserId();

            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId),true)).FirstOrDefault();

            var enteredPasswordHash = await GetHashString($"{oldPassword}{user.SecurityStamp}");
            if (!enteredPasswordHash.Equals(user.PasswordHash))
                return new CommonResponse(false, "Check credetials you have entered! Wrong Email or Password!");

            user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "Success!");
        }

        public async Task<CommonResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPairDTO)
        {
            var isAdmin = await IsCurrentUserAdministrator();
            if (isAdmin == false)
                return new CommonResponse<UserDetailedDTO>(false, "Forbidden Action!", null);

            var role = (await _repositoryManager.Role.GetRolesWithExpressionAsync(r => r.Id.Equals(userIdRoleIdPairDTO.RoleId),false)).FirstOrDefault();
            if (role is null)
                return new CommonResponse(false, "No such Roles!");

            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(userIdRoleIdPairDTO.UserId), true)).FirstOrDefault();
            if (user is null)
                return new CommonResponse(false, "No such Users!");

            user.RoleId = userIdRoleIdPairDTO.RoleId;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "Success!");
        }

        public async Task<CommonResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        {
            var isAdmin = await IsCurrentUserAdministrator();
            if (isAdmin == false)
                return new CommonResponse<UserDetailedDTO>(false, "Forbidden Action!", null);

            var userStatus = (await _repositoryManager.UserStatus.GetUserStatusesWithExpressionAsync(us => us.Id.Equals(userIdUserStatusIdPairDTO.UserStatusId), false)).FirstOrDefault();
            if (userStatus is null)
                return new CommonResponse(false, "No such User Statuses!");

            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(userIdUserStatusIdPairDTO.UserId), true)).FirstOrDefault();
            if (user is null)
                return new CommonResponse(false, "No such Users!");

            user.UserStatusId = userIdUserStatusIdPairDTO.UserStatusId;
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, "Success!");
        }
    }
}
