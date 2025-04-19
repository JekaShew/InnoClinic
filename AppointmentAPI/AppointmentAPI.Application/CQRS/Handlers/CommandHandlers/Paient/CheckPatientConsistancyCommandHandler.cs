using AppointmentAPI.Application.CQRS.Commands.Patient;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Paient;

public class CheckPatientConsistancyCommandHandler : IRequestHandler<CheckPatientConsistancyCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckPatientConsistancyCommandHandler(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CheckPatientConsistancyCommand request, CancellationToken cancellationToken)
    {
        var patient = await _repositoryManager.Patient.GetByIdAsync(request.PatientCheckConsistancyEvent.Id);
        var consistantPatient = _mapper.Map<Domain.Data.Models.Patient>(request.PatientCheckConsistancyEvent);
        if (patient is null)
        {
            await _repositoryManager.Patient.CreateAsync(consistantPatient);
            _logger.Information($"Succesfully added Patient: {consistantPatient} !");
        }

        if (patient is not null)
        {
            patient = _mapper.Map(consistantPatient, patient);
            await _repositoryManager.Patient.UpdateAsync(patient.Id, patient);
            _logger.Information($"Succesfully updated Patient: {patient}");
        }
    }
}
