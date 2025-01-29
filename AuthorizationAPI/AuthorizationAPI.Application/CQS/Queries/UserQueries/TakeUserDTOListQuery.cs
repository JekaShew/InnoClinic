using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class TakeUserDTOListQuery : IRequest<List<UserDTO>>
    {
    }
}
