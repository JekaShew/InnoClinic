using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;

public class CreateServiceCategoryCommand : IRequest<ResponseMessage<ServiceCategoryInfoDTO>>
{
    public ServiceCategoryForCreateDTO? ServiceCategoryForCreateDTO { get; set; }
}
