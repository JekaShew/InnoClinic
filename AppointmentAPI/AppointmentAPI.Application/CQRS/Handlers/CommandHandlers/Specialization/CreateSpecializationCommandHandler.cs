using AppointmentAPI.Application.CQRS.Commands.Specialization;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Specialization;

public class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialziation = _mapper.Map<Domain.Data.Models.Specialization>(request.SpecializationCreatedEvent);
        await _repositoryManager.Specialization.CreateAsync(specialziation);
        await _repositoryManager.CommitAsync();
        _logger.Information($"Succesfully added Specialization: {specialziation}");
    }
}
