using AuthorizationAPI.Application.CQS.Queries.RoleQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RoleHandlers.QueriesHandlers
{
    public class TakeRoleDTOByIdQueryHandler : IRequestHandler<TakeRoleDTOByIdQuery, RoleDTO>
    {
        private readonly IRole _roleRepository;
        public TakeRoleDTOByIdQueryHandler(IRole roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleDTO> Handle(TakeRoleDTOByIdQuery request, CancellationToken cancellationToken)
        {
            return await _roleRepository.TakeRoleById(request.Id);
        }
    }
}
