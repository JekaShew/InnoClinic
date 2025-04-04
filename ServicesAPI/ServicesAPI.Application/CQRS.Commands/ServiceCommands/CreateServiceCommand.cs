using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCommands;

public class CreateServiceCommand : IRequest<ResponseMessage<ServiceInfoDTO>>
{
    public ServiceForCreateDTO? serviceForCreateDTO { get; set; }
}
