using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.SpecializationCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.SpecializationCommandHandlers;

public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, ResponseMessage<SpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ResponseMessage<SpecializationInfoDTO>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await _repositoryManager.Specialization.GetByIdAsync(request.SpecializationId);
        if (specialization is null)
        {
            return new ResponseMessage<SpecializationInfoDTO>("Specialization not Found!", 404);
        }

        specialization = _mapper.Map<Specialization>(request.specializationForUpdateDTO);

        await _repositoryManager.BeginAsync();
        var oldServiceCategorySpecializations = await _repositoryManager.ServiceCategorySpecialization
            .GetAllByExpressionAsync(scs => scs.SpecializationId.Equals(request.SpecializationId));
        var newServiceCategorySpecializations = new List<ServiceCategorySpecialization>();

        foreach(var oldServiceCategorySpecialization in oldServiceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.DeleteAsync(oldServiceCategorySpecialization);
        }

        if (request.specializationForUpdateDTO.ServiceCategories is not null
        && request.specializationForUpdateDTO.ServiceCategories.Count >= 1)
        {
            foreach (var newServiceCategoryId in request.specializationForUpdateDTO.ServiceCategories)
            {
                var serviceCategorySpecialization = new ServiceCategorySpecialization
                {
                    ServiceCategoryId = newServiceCategoryId,
                    SpecializationId = specialization.Id
                };
                newServiceCategorySpecializations.Add(serviceCategorySpecialization);
                // await _repositoryManager.ServiceCategorySpecialization.CreateAsync(serviceCategorySpercialization);
            }
        }

        foreach (var newServiceCategorySpercialization in newServiceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.CreateAsync(newServiceCategorySpercialization);
        }

        await _repositoryManager.Specialization.UpdateAsync(request.SpecializationId, specialization);
        await _repositoryManager.CommitAsync();
        var specializationUpdatedEvent = _mapper.Map<SpecializationUpdatedEvent>(specialization);
        await _publishEndpoint.Publish(specializationUpdatedEvent);
        var specializationInfoDTO = _mapper.Map<SpecializationInfoDTO>(specialization);

        return new ResponseMessage<SpecializationInfoDTO>(specializationInfoDTO);
    }
}
