using AuthorizationAPI.Application.CQS.Commands.UserStatusCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserStatusHandlers.CommandsHandlers
{
    public class DeleteUserStatusByIdCommandHandler : IRequestHandler<DeleteUserStatusByIdCommand, CustomResponse>
    {
        private readonly IUserStatus _userStatusRepository;
        public DeleteUserStatusByIdCommandHandler(IUserStatus userStatusRepository)
        {
            _userStatusRepository = userStatusRepository; 
        }
        public async Task<CustomResponse> Handle(DeleteUserStatusByIdCommand request, CancellationToken cancellationToken)
        {
            return await _userStatusRepository.DeleteUserStatusById(request.Id);
        }
    }
}
