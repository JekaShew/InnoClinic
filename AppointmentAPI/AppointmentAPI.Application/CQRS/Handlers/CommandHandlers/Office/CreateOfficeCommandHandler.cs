using AppointmentAPI.Application.CQRS.Commands.Office;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Office;

public class CreateOfficeCommandHandler : IRequestHandler<CreateOfficeCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateOfficeCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CreateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = _mapper.Map<Domain.Data.Models.Office>(request.OfficeCreatedEvent);
        await _repositoryManager.Office.CreateAsync(office);
        await _repositoryManager.CommitAsync();
        _logger.Information($"Succesfully added Office: {office}");
    }
}
