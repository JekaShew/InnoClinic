using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.CommandsHandlers
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, CustomResponse>
    {
        private readonly IUser _userRepository;
        private readonly IUserServices _userServices;
        public ChangePasswordCommandHandler(IUser userRepository, IUserServices userServices)
        {
            _userRepository = userRepository;
            _userServices = userServices;
        }
        public async Task<CustomResponse> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var authorizationInfoDTO = await _userRepository.TakeAuthorizationInfoByUserId(request.UserId);
            var newPasswordHash = await _userServices.GetHashString($"{request.NewPassword}{authorizationInfoDTO.SecurityStamp}");

            authorizationInfoDTO.PasswordHash = newPasswordHash;

            return await _userRepository.UpdateAuthorizationInfoOfUser(authorizationInfoDTO);
        }
    }
}
