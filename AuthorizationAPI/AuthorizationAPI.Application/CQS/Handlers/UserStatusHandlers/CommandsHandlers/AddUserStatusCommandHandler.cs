using AuthorizationAPI.Application.CQS.Commands.UserStatusCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserStatusHandlers.CommandsHandlers
{
    public class AddUserStatusCommandHandler : IRequestHandler<AddUserStatusCommand, CustomResponse>
    {
        private readonly IUserStatus _userStatusRepository;
        public AddUserStatusCommandHandler(IUserStatus userStatusRepository)
        {
            _userStatusRepository = userStatusRepository;
        }
        public async Task<CustomResponse> Handle(AddUserStatusCommand request, CancellationToken cancellationToken)
        {
            request.UserStatusDTO.Id = Guid.NewGuid();

            return await _userStatusRepository.AddUserStatus(request.UserStatusDTO);
        }
    }
}
