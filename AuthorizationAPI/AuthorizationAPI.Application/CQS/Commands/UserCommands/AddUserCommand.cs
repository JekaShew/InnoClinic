using AuthorizationAPI.Application.DTOs;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.UserCommands
{
    public class AddUserCommand : IRequest<CustomResponse>
    {
        public RegistrationInfoDTO RegistrationInfoDTO { get; set; }
    }
}
