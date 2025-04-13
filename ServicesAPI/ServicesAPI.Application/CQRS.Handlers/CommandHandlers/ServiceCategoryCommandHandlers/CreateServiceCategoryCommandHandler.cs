using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCategoryCommandHandlers;

public class CreateServiceCategoryCommandHandler : IRequestHandler<CreateServiceCategoryCommand, ResponseMessage<ServiceCategoryInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public CreateServiceCategoryCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceCategoryInfoDTO>> Handle(CreateServiceCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = _mapper.Map<ServiceCategory>(request.ServiceCategoryForCreateDTO);
        await _repositoryManager.BeginAsync();
        await _repositoryManager.ServiceCategory.CreateAsync(serviceCategory);
        var serviceCategorySpecializations = new List<ServiceCategorySpecialization>();
        if (request.ServiceCategoryForCreateDTO.Specializations is not null
            && request.ServiceCategoryForCreateDTO.Specializations.Count >= 1)
        {
            foreach (var specializtionId in request.ServiceCategoryForCreateDTO.Specializations)
            {
                var serviceCategorySpecialization = new ServiceCategorySpecialization
                {
                    ServiceCategoryId = serviceCategory.Id,
                    SpecializationId = specializtionId
                };
                serviceCategorySpecializations.Add(serviceCategorySpecialization);
            }
        }

        foreach (var serviceCategorySpercialization in serviceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.CreateAsync(serviceCategorySpercialization);
        }

        await _repositoryManager.CommitAsync();
        var serviceCategoryInfoDTO = _mapper.Map<ServiceCategoryInfoDTO>(serviceCategory);

        return new ResponseMessage<ServiceCategoryInfoDTO>(serviceCategoryInfoDTO);
    }
}
