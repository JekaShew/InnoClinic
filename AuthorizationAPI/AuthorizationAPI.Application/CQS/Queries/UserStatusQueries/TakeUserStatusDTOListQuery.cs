using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserStatusQueries
{
    public class TakeUserStatusDTOListQuery : IRequest<List<UserStatusDTO>>
    {
    }
}
