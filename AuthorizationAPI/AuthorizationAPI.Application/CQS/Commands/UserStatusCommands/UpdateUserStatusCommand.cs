using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserStatusCommands
{
    public class UpdateUserStatusCommand : IRequest<CustomResponse>
    {
        public UserStatusDTO UserStatusDTO { get; set; }
    }
}
