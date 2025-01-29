using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.CommandsHandlers
{
    public class ChangeRoleOfUserCommandHandler : IRequestHandler<ChangeRoleOfUserCommand, CustomResponse>
    {
        private readonly IUser _userRepository;
        private readonly IRole _roleRepository;
        public ChangeRoleOfUserCommandHandler(IUser userRepository, IRole roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<CustomResponse> Handle(ChangeRoleOfUserCommand request, CancellationToken cancellationToken)
        {
            var roleDTO = await _roleRepository.TakeRoleById(request.UserIdRoleIdPairDTO.RoleId);
            if (roleDTO is null)
                return new CustomResponse(false, "No such Roles!");

            var userDTO = await _userRepository.TakeUserById(request.UserIdRoleIdPairDTO.UserId);
            if (userDTO is null)
                return new CustomResponse(false, "No such Users!");

            userDTO.UserStatusId = request.UserIdRoleIdPairDTO.RoleId;

            return await _userRepository.UpdateUser(userDTO);
        }
    }
}
