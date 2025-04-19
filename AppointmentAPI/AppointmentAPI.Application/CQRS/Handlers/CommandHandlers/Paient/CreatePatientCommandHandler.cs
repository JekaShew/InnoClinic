using AppointmentAPI.Application.CQRS.Commands.Patient;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Paient;

public class CreatePatientCommandHandler : IRequestHandler<CreatePatientCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreatePatientCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CreatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = _mapper.Map<Domain.Data.Models.Patient>(request.PatientCreatedEvent);
        await _repositoryManager.Patient.CreateAsync(patient);
        await _repositoryManager.CommitAsync();
        _logger.Information($"Succesfully added Patient: {patient} !");
    }
}
