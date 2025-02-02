using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.DTOs;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserStatusRepository _userStatusRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserServices(
                IHttpContextAccessor httpContextAccessor, 
                IUserRepository userRepository, 
                IUserStatusRepository userStatusRepository, 
                IRoleRepository roleRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _userStatusRepository = userStatusRepository;
            _roleRepository = roleRepository;
        }

        public Guid? TakeCurrentUserId()
        {
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
        // Add Common methods isAdmin ChangeEmail ActivateEmail-> default UserStatus change to "Not Activated"

        public async Task<bool> IsCurrentUserAdministrator()
        {
            var currentUserId = TakeCurrentUserId();
            var adminRoleId = await _roleRepository.TakeRoleWithPredicate(r => r.Title == "Administrator");
            if (adminRoleId is null || adminRoleId.Equals(Guid.Empty))
                return false;

            var user = await _userRepository.TakeUserById(currentUserId.Value);
            if (!user.Value.RoleId.Equals(adminRoleId))
                return false;
            return true;
        }

        public async Task<CustomResponse> CreateUser(RegistrationInfoDTO registrationInfoDTO)
        {
            var user = await _userRepository.TakeUserWithPredicate(u => u.Email.Equals(registrationInfoDTO.Email));
            if (user.Flag == true)
                return new CustomResponse(false, $"Email {registrationInfoDTO.Email} has been already registered!");

            var defaultRole = await _roleRepository.TakeRoleWithPredicate(r => r.Title == "Patient");
            if (defaultRole.Value == null || defaultRole.Value.Id == Guid.Empty)
                return new CustomResponse(false, "There is no Default Role named Patient in DB!");

            var defaultUserStatus = await _userStatusRepository.TakeUserStatusWithPredicate(us => us.Title == "Activated");
            if (defaultUserStatus.Value == null && defaultUserStatus.Value.Id == Guid.Empty)
                return new CustomResponse(false, "There is no Default User Status named Acitvated in DB!");

            var securityStamp = await GetHashString(registrationInfoDTO.SecretPhrase);
            var secretPhraseHash = await GetHashString($"{registrationInfoDTO.SecretPhrase}{securityStamp}");
            var passwordHash = await GetHashString($"{registrationInfoDTO.Password}{securityStamp}");

            var newUser = UserMapper.RegistrationInfoDTOToUser(registrationInfoDTO);
            newUser.Id = Guid.NewGuid();
            newUser.RoleId = defaultRole.Value.Id;
            newUser.UserStatusId = defaultUserStatus.Value.Id;
            newUser.SecurityStamp = securityStamp;
            newUser.SecretPhraseHash = secretPhraseHash;
            newUser.PasswordHash = passwordHash;

            return await _userRepository.AddUser(newUser);
        }

        public async Task<CustomResponse> DeleteAccountById()
        {
            var deletedUserStatus = await _userStatusRepository.TakeUserStatusWithPredicate(r => r.Title == "Deleted");
            if(deletedUserStatus.Flag == false  
                || deletedUserStatus.Value.Id.Equals(Guid.Empty))
                return new CustomResponse(false, "There is no Default User Status named Deleted in DB!");
            
            var userId = TakeCurrentUserId();

            return await ChangeUserStatusOfUser(
                new UserIdUserStatusIdPairDTO()
                {
                    UserId = userId.Value,
                    UserStatusId = deletedUserStatus.Value.Id
                }); 
        }

        public async Task<CustomResponse> DeleteUserById(Guid userId)
        {
            var isAdmin = await IsCurrentUserAdministrator();
            if(isAdmin == false)
                return new CustomResponse(false, "Forbidden Action!");

            return await _userRepository.DeleteUserById(userId);
        }

        public async Task<CustomResponse<List<UserInfoDTO>>> TakeAllUsersInfo()
        {
            var users = await _userRepository.TakeAllUsers();
            var userInfoDTOs = users.Value.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();
            
            return new CustomResponse<List<UserInfoDTO>>(true,"Success!", userInfoDTOs);
        }

        public async Task<CustomResponse<UserDetailedDTO>> GetUserDetailedInfo(Guid userId)
        {
            var isAdmin = await IsCurrentUserAdministrator();
            if(isAdmin == false)
                return new CustomResponse<UserDetailedDTO>(false, "Forbidden Action!",null);

            var user = await _userRepository.TakeUserById(userId);
            var userDetailedInfoDTO = UserMapper.UserToUserDetailedDTO(user.Value);

            return new CustomResponse<UserDetailedDTO>(true, "Success!", userDetailedInfoDTO);
        }

        public async Task<CustomResponse> EditUserInfo(UserInfoDTO userInfoDTO)
        {
            var currentUserId = TakeCurrentUserId();
            var user = await _userRepository.TakeUserWithPredicate(u => u.Email.Equals(userInfoDTO.Email));
            if (!user.Value.Id.Equals(currentUserId.Value))
                return new CustomResponse(false, "Forbidden Action!");

            var updatedUser = UserMapper.UserInfoDTOToUser(userInfoDTO);
            updatedUser.Id = user.Value.Id;

            return await _userRepository.UpdateUser(updatedUser);
        }

        //Implement
        public Task<CustomResponse> ChangeForgottenPasswordByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var user = await _userRepository.TakeUserWithPredicate(u => u.Email.Equals(emailSecretPhrasePairDTO.Email));
            if (user is null)
                return new CustomResponse(false, "User not Found!");

            var enteredSecretPhraseHash = await GetHashString($"{emailSecretPhrasePairDTO.SecretPhrase}{user.Value.SecurityStamp}");

            if (!enteredSecretPhraseHash.Equals(user.Value.SecretPhraseHash))
                return new CustomResponse(false, "Check credetials you have entered! Wrong Email or Secret Phrase!");

            user.Value.PasswordHash = await GetHashString($"{user.Value.PasswordHash}{user.Value.SecurityStamp}");

            return await _userRepository.UpdateUser(user.Value);
        }

        public async Task<CustomResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword)
        {
            var userId = TakeCurrentUserId();

            var user = await _userRepository.TakeUserById(userId.Value);

            var enteredPasswordHash = await GetHashString($"{oldPassword}{user.Value.SecurityStamp}");
            if (!enteredPasswordHash.Equals(user.Value.PasswordHash))
                return new CustomResponse(false, "Check credetials you have entered! Wrong Email or Password!");

            user.Value.PasswordHash = await GetHashString($"{user.Value.PasswordHash}{user.Value.SecurityStamp}");

            return await _userRepository.UpdateUser(user.Value); 
        }

        public async Task<CustomResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPairDTO)
        {
            var role = await _roleRepository.TakeRoleById(userIdRoleIdPairDTO.RoleId);
            if (role is null)
                return new CustomResponse(false, "No such Roles!");

            var user = await _userRepository.TakeUserById(userIdRoleIdPairDTO.UserId);
            if (user is null)
                return new CustomResponse(false, "No such Users!");

            user.Value.RoleId = userIdRoleIdPairDTO.RoleId;

            return await _userRepository.UpdateUser(user.Value);
        }

        public async Task<CustomResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        {
            var userStatus = await _userStatusRepository.TakeUserStatusById(userIdUserStatusIdPairDTO.UserStatusId);
            if (userStatus is null)
                return new CustomResponse(false, "No such User Statuses!");

            var user = await _userRepository.TakeUserById(userIdUserStatusIdPairDTO.UserId);
            if (user is null)
                return new CustomResponse(false, "No such Users!");

            user.Value.UserStatusId = userIdUserStatusIdPairDTO.UserStatusId;

            return await _userRepository.UpdateUser(user.Value);
        }
    }
}
