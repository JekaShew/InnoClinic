using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;

public class DeleteServiceCategoryCommand : IRequest<ResponseMessage>
{
    public Guid Id { get; set; }
}
