using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Commands.SpecializationCommands;

public class CreateSpecializationCommand : IRequest<ResponseMessage<SpecializationInfoDTO>>
{
    public SpecializationForCreateDTO? specializationForCreateDTO { get; set; }
}
