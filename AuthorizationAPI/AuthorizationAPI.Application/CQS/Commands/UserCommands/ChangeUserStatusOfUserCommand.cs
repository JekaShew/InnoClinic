using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserCommands
{
    public class ChangeUserStatusOfUserCommand : IRequest<CustomResponse>
    {
        public UserIdUserStatusIdPairDTO UserIdUserStatusIdPairDTO {  get; set; }
    }
}
