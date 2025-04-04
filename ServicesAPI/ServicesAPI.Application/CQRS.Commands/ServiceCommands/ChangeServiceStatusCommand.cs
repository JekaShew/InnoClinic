using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCommands;

public class ChangeServiceStatusCommand : IRequest<ResponseMessage>
{
    public Guid ServiceId { get; set; }
}
