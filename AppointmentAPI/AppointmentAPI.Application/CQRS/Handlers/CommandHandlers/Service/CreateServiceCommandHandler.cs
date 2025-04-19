using AppointmentAPI.Application.CQRS.Commands.Service;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Service;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateServiceCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = _mapper.Map<Domain.Data.Models.Service>(request.ServiceCreatedEvent);
        await _repositoryManager.Service.CreateAsync(service);
        await _repositoryManager.CommitAsync();
        _logger.Information($"Succesfully added Service: {service}");
    }
}
