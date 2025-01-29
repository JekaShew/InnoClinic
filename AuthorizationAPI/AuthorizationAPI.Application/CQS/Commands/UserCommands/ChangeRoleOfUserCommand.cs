using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserCommands
{
    public class ChangeRoleOfUserCommand : IRequest<CustomResponse>
    {
        public UserIdRoleIdPairDTO UserIdRoleIdPairDTO { get; set; }
    }
}
