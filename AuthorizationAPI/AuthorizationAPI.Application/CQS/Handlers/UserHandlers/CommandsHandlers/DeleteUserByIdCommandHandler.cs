using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.CommandsHandlers
{
    public class DeleteUserByIdCommandHandler : IRequestHandler<DeleteUserByIdCommand, CustomResponse>
    {
        private readonly IUser _userRepository;
        public DeleteUserByIdCommandHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<CustomResponse> Handle(DeleteUserByIdCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteUserById(request.Id);
        }
    }
}
