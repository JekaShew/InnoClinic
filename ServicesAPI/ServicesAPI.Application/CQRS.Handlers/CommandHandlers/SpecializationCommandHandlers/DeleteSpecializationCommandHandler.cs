using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.SpecializationCommands;
using ServicesAPI.Domain.Data.IRepositories;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.SpecializationCommandHandlers;

public class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteSpecializationCommandHandler(IRepositoryManager repositoryManager, IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;
    }

    public async Task<ResponseMessage> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await _repositoryManager.Specialization.GetByIdAsync(request.Id);
        if (specialization is null)
        {
            return new ResponseMessage("Specvialization not Found!", 404);
        }

        await _repositoryManager.BeginAsync();
        // Delete all ServiceCategorySpecializations related to this specialization
        var serviceCategorySpecializations = await _repositoryManager.ServiceCategorySpecialization
            .GetAllByExpressionAsync(scs => scs.SpecializationId.Equals(request.Id));

        foreach (var serviceCategorySpecialization in serviceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.DeleteAsync(serviceCategorySpecialization);
        }
        
        await _repositoryManager.Specialization.DeleteAsync(specialization);
        await _repositoryManager.CommitAsync();
        var specializationDeletedEvent = _mapper.Map<SpecializationDeletedEvent>(specialization);
        await _publishEndpoint.Publish(specializationDeletedEvent);

        return new ResponseMessage();
    }
}
