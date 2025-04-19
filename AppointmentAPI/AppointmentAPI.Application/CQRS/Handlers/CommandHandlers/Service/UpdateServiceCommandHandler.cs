using AppointmentAPI.Application.CQRS.Commands.Service;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Service;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateServiceCommandHandler(
            IRepositoryManager repositoryManager,
            ILogger logger,
            IMapper mapper)
    {
        _logger = logger;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = _mapper.Map<Domain.Data.Models.Service>(request.ServiceUpdatedEvent);
        if (service is not null)
        {
            await _repositoryManager.Service.UpdateAsync(request.ServiceUpdatedEvent.Id, service);
            await _repositoryManager.CommitAsync();
            _logger.Information($"Succesfully updated Service: {service} !");
        }
    }
}
