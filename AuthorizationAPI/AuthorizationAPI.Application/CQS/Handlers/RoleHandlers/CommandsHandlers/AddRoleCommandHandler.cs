using AuthorizationAPI.Application.CQS.Commands.RoleCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RoleHandlers.CommandsHandlers
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, CustomResponse>
    {
        private readonly IRole _roleRepository;
        public AddRoleCommandHandler(IRole roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<CustomResponse> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            request.RoleDTO.Id = Guid.NewGuid();

            return await _roleRepository.AddRole(request.RoleDTO);
        }
    }
}
