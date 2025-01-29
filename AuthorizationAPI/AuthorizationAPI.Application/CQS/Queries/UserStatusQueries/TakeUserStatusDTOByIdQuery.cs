using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserStatusQueries
{
    public class TakeUserStatusDTOByIdQuery : IRequest<UserStatusDTO>
    {
        public Guid Id { get; set; }
    }
}
