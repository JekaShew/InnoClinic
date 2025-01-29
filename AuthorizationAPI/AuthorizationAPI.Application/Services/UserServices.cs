using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthorizationAPI.Application.Services
{
    public class UserServices : IUserServices
    {
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserServices(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<CustomResponse> ChangeForgottenPasswordBySecretPhrase(EmailSecretPhrasePairDTO emailSecretPhrasePairDTO, string newPassword)
        {
            var isLoginRegistered = await _mediator.Send(new IsEmailRegisteredQuery() { EnteredEmail = emailSecretPhrasePairDTO.Email });
            if(isLoginRegistered.Flag == false)
                return isLoginRegistered;

            var userId = await _mediator.Send(new TakeUserIdByEmailQuery() { Email = emailSecretPhrasePairDTO.Email });

            var checkEmailSecretPhrasePair = await _mediator.Send(new CheckEmailSecretPhrasePairQuery() { EmailSecretPhrasePairDTO = emailSecretPhrasePairDTO });
            if(checkEmailSecretPhrasePair.Flag == false)
                return checkEmailSecretPhrasePair;

            return await _mediator.Send(new ChangePasswordCommand() { UserId = userId, NewPassword = newPassword }); 
        }
        //Implement
        public Task<CustomResponse> ChangeForgottenPasswordByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResponse> ChangePasswordByOldPassword(string oldPassword, string newPassword)
        {
            var userId = TakeCurrentUserId();
            var userEmail = await _mediator.Send(new TakeUserEmailByUserIdQuery() { UserId = userId.Value });

            var checkEmailPasswordPair = await _mediator.Send(new CheckEmailPasswordPairQuery() { LoginInfoDTO = { Email = userEmail, Password = oldPassword } });
            if(checkEmailPasswordPair.Flag == false)
                return checkEmailPasswordPair;

            return await _mediator.Send(new ChangePasswordCommand() { UserId = userId.Value, NewPassword = newPassword });
        }

        public async Task<CustomResponse> ChangeRoleOfUser(UserIdRoleIdPairDTO userIdRoleIdPairDTO)
        {
            return await _mediator.Send(new ChangeRoleOfUserCommand() { UserIdRoleIdPairDTO = userIdRoleIdPairDTO });
        }

        public async Task<CustomResponse> ChangeUserStatusOfUser(UserIdUserStatusIdPairDTO userIdUserStatusIdPairDTO)
        {
            return await _mediator.Send(new ChangeUserStatusOfUserCommand() { UserIdUserStatusIdPairDTO = userIdUserStatusIdPairDTO });
        }
    }
}
