using AppointmentAPI.Application.CQRS.Commands.Patient;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Paient;

public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdatePatientCommandHandler(
            IRepositoryManager repositoryManager,
            ILogger logger,
            IMapper mapper)
    {
        _logger = logger;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task Handle(UpdatePatientCommand request, CancellationToken cancellationToken)
    {
        var patient = _mapper.Map<Domain.Data.Models.Patient>(request.PatientUpdatedEvent);
        if (patient is not null)
        {
            await _repositoryManager.Patient.UpdateAsync(request.PatientUpdatedEvent.Id, patient);
            await _repositoryManager.CommitAsync();
            _logger.Information($"Succesfully updated Patient: {patient}");
        }
    }
}
