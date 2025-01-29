using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.RoleQueries
{
    public class TakeRoleDTOByIdQuery : IRequest<RoleDTO>
    {
        public Guid Id { get; set; }
    }
}
