using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace ServicesAPI.Application.CQRS.Commands.SpecializationCommands;

public class DeleteSpecializationCommand : IRequest<ResponseMessage>
{
    public Guid Id { get; set; }
}
