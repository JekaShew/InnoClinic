using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCommands;

public class UpdateServiceCommand : IRequest<ResponseMessage<ServiceInfoDTO>>
{
    public Guid ServiceId { get; set; }
    public ServiceForUpdateDTO? serviceForUpdateDTO { get; set; }
}
