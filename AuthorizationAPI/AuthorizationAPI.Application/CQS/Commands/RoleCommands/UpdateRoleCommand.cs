using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.RoleCommands
{
    public class UpdateRoleCommand : IRequest<CustomResponse>
    {
        public RoleDTO RoleDTO { get; set; }
    }
}
