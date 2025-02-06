using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Services.Mappers;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IValidator<OldNewPasswordPairDTO> _oldNewPasswordPairDTOValidator;
        private readonly IValidator<EmailSecretPhrasePairDTO> _emailSecretPhrasePairDTOValidator;
        private readonly IValidator<UserForUpdateDTO> _userForUpdateDTOValidator;

        private readonly IRepositoryManager _repositoryManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(
                IHttpContextAccessor httpContextAccessor,
                IRepositoryManager repositoryManager,
                IValidator<OldNewPasswordPairDTO> oldNewPasswordPairDTOValidator,
                IValidator<EmailSecretPhrasePairDTO> emailSecretPhrasePairDTOValidator,
                IValidator<UserForUpdateDTO> userForUpdateDTOValidator)
        {
            _httpContextAccessor = httpContextAccessor;
            _repositoryManager = repositoryManager;
            _oldNewPasswordPairDTOValidator = oldNewPasswordPairDTOValidator;
            _emailSecretPhrasePairDTOValidator = emailSecretPhrasePairDTOValidator;
            _userForUpdateDTOValidator = userForUpdateDTOValidator;
        }
        // Add methods ChangeOldEmailByEmail
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

        public async Task<UserDetailedDTO> IsCurrentUserAdministrator()
        {
            var currentUserId = TakeCurrentUserId();
            var adminRole = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(DBConstants.AdministratorRoleId), false))
                    .FirstOrDefault();
            if (adminRole is null || adminRole!.Id.Equals(Guid.Empty))
                return null;

            var currentUser = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId), false))
                    .FirstOrDefault();
            if (currentUser is null || !currentUser.RoleId.Equals(adminRole.Id))
                return null;

            return UserMapper.UserToUserDetailedDTO(currentUser)!;
        }

        public async Task<UserDetailedDTO> IsEmailRegistered(string email, bool trackChanges)
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
            newUser.RoleId = defaultRole.Id;
            newUser.UserStatusId = defaultUserStatus.Id;
            newUser.SecurityStamp = securityStamp;
            newUser.SecretPhraseHash = secretPhraseHash;
            newUser.PasswordHash = passwordHash;

            _repositoryManager.User.CreateUser(newUser);
            await _repositoryManager.SaveChangesAsync();

            return newUser.Id;
        }

        public async Task<ResponseMessage> DeleteCurrentAccount()
        {
            var deletedUserStatus = (await _repositoryManager
                    .UserStatus
                    .GetUserStatusesWithExpressionAsync(r => r.Id.Equals(DBConstants.DeletedUserStatusId), false))
                    .FirstOrDefault();
            if (deletedUserStatus is null || deletedUserStatus.Id.Equals(Guid.Empty))
                return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);

            var currentUserId = TakeCurrentUserId();
            var currentUser = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId), true))
                    .FirstOrDefault();
            currentUser.UserStatusId = deletedUserStatus.Id;
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }

        public async Task<ResponseMessage> DeleteUserById(Guid userId)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            var userToDelete = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userId), false))
                    .FirstOrDefault();
            _repositoryManager.User.DeleteUser(userToDelete);
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessDeleteMessage, true);
        }

        public async Task<ResponseMessage<List<UserInfoDTO>>> GetAllUsersInfo()
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new ResponseMessage<List<UserInfoDTO>>(MessageConstants.ForbiddenMessage, false);

            var users = await _repositoryManager.User.GetAllUsersAsync(false);
            if (!users.Any())
                return new ResponseMessage<List<UserInfoDTO>>(MessageConstants.NotFoundMessage, false);

            var userInfoDTOs = users.Select(u => UserMapper.UserToUserInfoDTO(u)).ToList();

            return new ResponseMessage<List<UserInfoDTO>>(MessageConstants.SuccessMessage, true, userInfoDTOs);
        }

        public async Task<ResponseMessage<UserDetailedDTO>> GetUserDetailedInfo(Guid userId)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new ResponseMessage<UserDetailedDTO>(MessageConstants.ForbiddenMessage, false);

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userId), false))
                    .FirstOrDefault();
            if (user is null)
                return new ResponseMessage<UserDetailedDTO>(MessageConstants.NotFoundMessage, false);

            var userDetailedInfoDTO = UserMapper.UserToUserDetailedDTO(user);

            return new ResponseMessage<UserDetailedDTO>(MessageConstants.SuccessMessage, true, userDetailedInfoDTO);
        }

        public async Task<ResponseMessage> UpdateUserInfo(Guid userId, UserForUpdateDTO userForUpdateDTO)
        {
            var validationResult = await _userForUpdateDTOValidator.ValidateAsync(userForUpdateDTO);
            if (!validationResult.IsValid)
                throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());

            var currentUserId = TakeCurrentUserId();
            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userId), true))
                    .FirstOrDefault();
            if (user is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            if (!user.Id.Equals(currentUserId.Value))
                return new ResponseMessage(MessageConstants.ForbiddenMessage, false);

            user = UserMapper.UserForUpdateDTOToUser(userForUpdateDTO);
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }
        //Implement
        public Task<ResponseMessage> ChangeEmail(string password, string newEmail)
        {
            //check currentUser??
            //check password
            //send activate code to newEmail
            throw new NotImplementedException();
        }
        //Implement
        public Task<ResponseMessage> ChangeForgottenPasswordByEmail(string email)
        {
            // send activate code to newEmail
            throw new NotImplementedException();
        }

        public async Task<ResponseMessage> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var validationResult = await _emailSecretPhrasePairDTOValidator.ValidateAsync(emailSecretPhrasePairDTO);
            if (!validationResult.IsValid)
                throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Email.Equals(emailSecretPhrasePairDTO.Email), true))
                    .FirstOrDefault();
            if (user is null)
                return new ResponseMessage(MessageConstants.CheckCredsMessage, false);

            var enteredSecretPhraseHash = await GetHashString($"{emailSecretPhrasePairDTO.SecretPhrase}{user.SecurityStamp}");
            if (!enteredSecretPhraseHash.Equals(user.SecretPhraseHash))
                return new ResponseMessage(MessageConstants.CheckCredsMessage, false);

            user.PasswordHash = await GetHashString($"{newPassword}{user.SecurityStamp}");
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }

        public async Task<ResponseMessage> ChangePasswordByOldPassword(OldNewPasswordPairDTO oldNewPasswordPairDTO)
        {
            var validationResult = await _oldNewPasswordPairDTOValidator.ValidateAsync(oldNewPasswordPairDTO);
            if (!validationResult.IsValid)
                throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());

            var currentUserId = TakeCurrentUserId();
            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(currentUserId), true))
                    .FirstOrDefault();
            var enteredPasswordHash = await GetHashString($"{oldNewPasswordPairDTO.OldPassword}{user.SecurityStamp}");
            if (!enteredPasswordHash.Equals(user.PasswordHash))
                return new ResponseMessage(MessageConstants.CheckCredsMessage, false);

            user.PasswordHash = await GetHashString($"{oldNewPasswordPairDTO.NewPassword}{user.SecurityStamp}");
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }

        public async Task<ResponseMessage> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPairDTO)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new ResponseMessage<UserDetailedDTO>(MessageConstants.ForbiddenMessage, false);

            var role = (await _repositoryManager.Role
                    .GetRolesWithExpressionAsync(r => r.Id.Equals(userIdRoleIdPairDTO.RoleId), false))
                    .FirstOrDefault();
            if (role is null)
                return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userIdRoleIdPairDTO.UserId), true))
                    .FirstOrDefault();
            if (user is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            user.RoleId = userIdRoleIdPairDTO.RoleId;
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }

        public async Task<ResponseMessage> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        {
            var adminUser = await IsCurrentUserAdministrator();
            if (adminUser is null)
                return new ResponseMessage<UserDetailedDTO>(MessageConstants.ForbiddenMessage, false);

            var userStatus = (await _repositoryManager.UserStatus
                    .GetUserStatusesWithExpressionAsync(us => us.Id.Equals(userIdUserStatusIdPairDTO.UserStatusId), false))
                    .FirstOrDefault();
            if (userStatus is null)
                return new ResponseMessage(MessageConstants.CheckDBDataMessage, false);

            var user = (await _repositoryManager.User
                    .GetUsersWithExpressionAsync(u => u.Id.Equals(userIdUserStatusIdPairDTO.UserId), true))
                    .FirstOrDefault();
            if (user is null)
                return new ResponseMessage(MessageConstants.NotFoundMessage, false);

            user.UserStatusId = userIdUserStatusIdPairDTO.UserStatusId;
            await _repositoryManager.SaveChangesAsync();

            return new ResponseMessage(MessageConstants.SuccessMessage, true);
        }
    }
}
