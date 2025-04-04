using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCommands;

public class DeleteServiceCommand : IRequest<ResponseMessage>
{
    public Guid Id { get; set; }
}
