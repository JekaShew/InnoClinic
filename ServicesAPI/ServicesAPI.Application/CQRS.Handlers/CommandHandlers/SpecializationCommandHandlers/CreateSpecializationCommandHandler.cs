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

public class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand, ResponseMessage<SpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ResponseMessage<SpecializationInfoDTO>> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = _mapper.Map<Specialization>(request.SpecializationForCreateDTO);
        
        await _repositoryManager.BeginAsync();
        await _repositoryManager.Specialization.CreateAsync(specialization);
        var serviceCategorySpecializations = new List<ServiceCategorySpecialization>();
        if (request.SpecializationForCreateDTO.ServiceCategories is not null 
            && request.SpecializationForCreateDTO.ServiceCategories.Count >= 1)
        {   
            foreach(var serviceCategoryId in request.SpecializationForCreateDTO.ServiceCategories)
            {
                var serviceCategorySpecialization = new ServiceCategorySpecialization
                {
                    ServiceCategoryId = serviceCategoryId,
                    SpecializationId = specialization.Id
                };
                serviceCategorySpecializations.Add(serviceCategorySpecialization);
                // await _repositoryManager.ServiceCategorySpecialization.CreateAsync(serviceCategorySpercialization);
            }
        }

        foreach(var serviceCategorySpercialization in serviceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.CreateAsync(serviceCategorySpercialization);
        }

        await _repositoryManager.CommitAsync();
        var specializationCreatedEvent = _mapper.Map<SpecializationCreatedEvent>(specialization);
        await _publishEndpoint.Publish(specializationCreatedEvent);
        var specializationInfoDTO = _mapper.Map<SpecializationInfoDTO>(specialization);

        return new ResponseMessage<SpecializationInfoDTO>(specializationInfoDTO);
    }
}
