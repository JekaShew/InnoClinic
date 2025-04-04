using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;

public class UpdateServiceCategoryCommand : IRequest<ResponseMessage<ServiceCategoryInfoDTO>>
{
    public Guid ServiceCategoryId { get; set; }
    public ServiceCategoryForUpdateDTO? serviceCategoryForUpdateDTO { get; set; }
}

