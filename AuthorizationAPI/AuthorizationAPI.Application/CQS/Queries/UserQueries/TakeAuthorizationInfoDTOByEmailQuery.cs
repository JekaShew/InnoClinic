using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class TakeAuthorizationInfoDTOByEmailQuery : IRequest<AuthorizationInfoDTO>
    {
        public string Email { get; set; }
    }
}
