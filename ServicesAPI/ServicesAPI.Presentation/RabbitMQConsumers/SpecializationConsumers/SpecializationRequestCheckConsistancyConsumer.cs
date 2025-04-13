using AutoMapper;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using MassTransit;
using MediatR;
using Serilog;
using ServicesAPI.Application.CQRS.Queries.SpecializationQueries;

namespace ServicesAPI.Presentation.RabbitMQConsumers.SpecializationConsumers;

public class SpecializationRequestCheckConsistancyConsumer : IConsumer<SpecializationRequestCheckConsistancyEvent>
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public SpecializationRequestCheckConsistancyConsumer(IMapper mapper, ILogger logger, IPublishEndpoint publishEndpoint, IMediator mediator)
    {
        _mapper = mapper;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _mediator = mediator;
    }
    public async Task Consume(ConsumeContext<SpecializationRequestCheckConsistancyEvent> context)
    {
        _logger.Information($"User with Id: {context.Message.UserId} requested check specializations consistancy on ProfilesAPI at {context.Message.DateTime}");
        var consistantSpecializations = await _mediator.Send(new GetAllSpecializationsQuery()); 
        foreach (var consistantSpecialization in consistantSpecializations)
        {
            var specializationCheckConsistancyEvent = _mapper.Map<SpecializationCheckConsistancyEvent>(consistantSpecialization);
            await _publishEndpoint.Publish(specializationCheckConsistancyEvent);
        }
    }
}
