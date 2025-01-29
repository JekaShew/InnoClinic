using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class TakeUserDTOByIdQuery : IRequest<UserDTO>
    {
        public Guid Id { get; set; }
    }
}
