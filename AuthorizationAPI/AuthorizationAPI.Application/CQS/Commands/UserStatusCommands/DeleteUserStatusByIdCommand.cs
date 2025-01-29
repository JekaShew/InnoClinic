using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserStatusCommands
{
    public class DeleteUserStatusByIdCommand : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
    }
}
