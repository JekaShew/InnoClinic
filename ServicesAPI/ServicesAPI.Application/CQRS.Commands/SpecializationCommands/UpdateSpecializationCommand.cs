using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Commands.SpecializationCommands;

public class UpdateSpecializationCommand : IRequest<ResponseMessage<SpecializationInfoDTO>>
{
    public Guid SpecializationId { get; set; }
    public SpecializationForUpdateDTO? specializationForUpdateDTO { get; set; }
}
