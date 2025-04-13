using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCategoryCommandHandlers;

public class UpdateServiceCategoryCommandHandler : IRequestHandler<UpdateServiceCategoryCommand, ResponseMessage<ServiceCategoryInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UpdateServiceCategoryCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceCategoryInfoDTO>> Handle(UpdateServiceCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = await _repositoryManager.ServiceCategory.GetByIdAsync(request.ServiceCategoryId);
        if(serviceCategory is null)
        {
            return new ResponseMessage<ServiceCategoryInfoDTO>("Service Category not Found!", 404);
        }

        serviceCategory = _mapper.Map(request.ServiceCategoryForUpdateDTO, serviceCategory);

        await _repositoryManager.BeginAsync();
        var oldServiceCategorySpecializations = await _repositoryManager.ServiceCategorySpecialization
            .GetAllByExpressionAsync(scs => scs.ServiceCategoryId.Equals(request.ServiceCategoryId));
        var newServiceCategorySpecializations = new List<ServiceCategorySpecialization>();

        foreach (var oldServiceCategorySpecialization in oldServiceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.DeleteAsync(oldServiceCategorySpecialization);
        }

        if (request.ServiceCategoryForUpdateDTO.Specialziations is not null
        && request.ServiceCategoryForUpdateDTO.Specialziations.Count >= 1)
        {
            foreach (var newSpecializationId in request.ServiceCategoryForUpdateDTO.Specialziations)
            {
                var serviceCategorySpecialization = new ServiceCategorySpecialization
                {
                    ServiceCategoryId = serviceCategory.Id,
                    SpecializationId = newSpecializationId
                };
                newServiceCategorySpecializations.Add(serviceCategorySpecialization);
            }
        }

        foreach (var newServiceCategorySpercialization in newServiceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.CreateAsync(newServiceCategorySpercialization);
        }

        await _repositoryManager.ServiceCategory.UpdateAsync(request.ServiceCategoryId, serviceCategory);
        await _repositoryManager.CommitAsync();
        var serviceCategoryInfoDTO = _mapper.Map<ServiceCategoryInfoDTO>(serviceCategory);

        return new ResponseMessage<ServiceCategoryInfoDTO>(serviceCategoryInfoDTO);
    }
}

