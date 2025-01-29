using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands
{
    public class DeleteRefreshTokenByRTokenIdCommand : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
    }
}
