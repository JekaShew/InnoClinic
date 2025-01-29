using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserCommands
{
    public class UpdateUserCommand : IRequest<CustomResponse>
    {
        public UserDTO UserDTO { get; set; }
    }
}
