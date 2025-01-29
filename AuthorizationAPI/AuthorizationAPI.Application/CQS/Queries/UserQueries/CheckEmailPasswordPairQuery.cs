using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class CheckEmailPasswordPairQuery : IRequest<CustomResponse>
    {
        public LoginInfoDTO LoginInfoDTO { get; set; }
    }
}
