using AuthorizationAPI.Application.CQS.Commands.RoleCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RoleHandlers.CommandsHandlers
{
    public class DeleteRoleByIdCommandHandler : IRequestHandler<DeleteRoleByIdCommand, CustomResponse>
    {
        private readonly IRole _roleRepository;
        public DeleteRoleByIdCommandHandler(IRole roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<CustomResponse> Handle(DeleteRoleByIdCommand request, CancellationToken cancellationToken)
        {
            return await _roleRepository.DeleteRoleById(request.Id);
        }
    }
}
