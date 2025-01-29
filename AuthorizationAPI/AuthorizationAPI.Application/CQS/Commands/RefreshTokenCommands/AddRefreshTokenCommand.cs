using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands
{
    public class AddRefreshTokenCommand : IRequest<CustomResponse>
    {
        public  RefreshTokenDTO RefreshTokenDTO { get; set; }
    }
}
