using AppointmentAPI.Application.CQRS.Commands.Service;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Service;

public class CheckServiceConsistancyCommandHandler : IRequestHandler<CheckServiceConsistancyCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckServiceConsistancyCommandHandler(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CheckServiceConsistancyCommand request, CancellationToken cancellationToken)
    {
        var service = await _repositoryManager.Service.GetByIdAsync(request.ServiceCheckConsistancyEvent.Id);
        var consistantService = _mapper.Map<Domain.Data.Models.Service>(request.ServiceCheckConsistancyEvent);
        if (service is null)
        {
            await _repositoryManager.Service.CreateAsync(consistantService);
            _logger.Information($"Succesfully added Service: {consistantService}");
        }

        if (service is not null)
        {
            service = _mapper.Map(consistantService, service);
            await _repositoryManager.Service.UpdateAsync(service.Id, service);
            _logger.Information($"Succesfully updated Service: {service} !");
        }
    }
}
