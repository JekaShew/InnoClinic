using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserCommands
{
    public class ChangePasswordCommand : IRequest<CustomResponse>
    {
        public Guid UserId { get; set; }
        public string NewPassword { get; set; }
    }
}
