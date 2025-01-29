using AuthorizationAPI.Application.CQS.Commands.RoleCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RoleHandlers.CommandsHandlers
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, CustomResponse>
    {
        private readonly IRole _roleRepository;
        public UpdateRoleCommandHandler(IRole roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<CustomResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            return await _roleRepository.UpdateRole(request.RoleDTO);
        }
    }
}
