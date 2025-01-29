using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.RoleCommands
{
    public class DeleteRoleByIdCommand : IRequest<CustomResponse>
    {
        public Guid Id { get; set; }
    }
}
