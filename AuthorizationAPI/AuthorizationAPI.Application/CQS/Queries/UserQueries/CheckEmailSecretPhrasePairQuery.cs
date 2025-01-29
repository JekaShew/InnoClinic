using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class CheckEmailSecretPhrasePairQuery : IRequest<CustomResponse>
    {
        public EmailSecretPhrasePairDTO EmailSecretPhrasePairDTO { get; set; }
    }
}
