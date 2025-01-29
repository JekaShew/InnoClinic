using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.RoleCommands
{
    public class AddRoleCommand : IRequest<CustomResponse>
    {
        public RoleDTO RoleDTO { get; set; }
    }
}
