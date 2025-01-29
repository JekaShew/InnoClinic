using AuthorizationAPI.Application.CQS.Commands.UserStatusCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserStatusHandlers.CommandsHandlers
{
    public class UpdateUserStatusCommandHandler : IRequestHandler<UpdateUserStatusCommand, CustomResponse>
    {
        private readonly IUserStatus _userStatusRepository;
        public UpdateUserStatusCommandHandler(IUserStatus userStatusRepository)
        {
            _userStatusRepository = userStatusRepository;
        }
        public async Task<CustomResponse> Handle(UpdateUserStatusCommand request, CancellationToken cancellationToken)
        {
            return await _userStatusRepository.UpdateUserStatus(request.UserStatusDTO);
        }
    }
}
