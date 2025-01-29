using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.CommandsHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, CustomResponse>
    {
        private readonly IUser _userRepository;
        public UpdateUserCommandHandler(IUser userRepository)
        {
            _userRepository = userRepository; 
        }
        public async Task<CustomResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateUser(request.UserDTO);
        }
    }
}
