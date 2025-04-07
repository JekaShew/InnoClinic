using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCategorySpecializationCommands;

public class UpdateServiceCategorySpecializationCommand : IRequest<ResponseMessage<ServiceCategorySpecializationInfoDTO>>
{
    public Guid ServiceCategorySpecializationId { get; set; }
    public ServiceCategorySpecializationForUpdateDTO? ServiceCategorySpecializationForUpdateDTO { get; set; }
}
