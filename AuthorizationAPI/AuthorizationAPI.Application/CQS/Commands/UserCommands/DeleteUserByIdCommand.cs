using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserCommands
{
    public class DeleteUserByIdCommand : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
    }
}
