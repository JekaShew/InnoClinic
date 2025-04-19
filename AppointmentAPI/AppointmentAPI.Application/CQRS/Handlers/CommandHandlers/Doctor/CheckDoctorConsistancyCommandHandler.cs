using AppointmentAPI.Application.CQRS.Commands.Doctor;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Doctor;

public class CheckDoctorConsistancyCommandHandler : IRequestHandler<CheckDoctorConsistancyCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckDoctorConsistancyCommandHandler(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CheckDoctorConsistancyCommand request, CancellationToken cancellationToken)
    {
        var doctor = await _repositoryManager.Doctor.GetByIdAsync(request.DoctorCheckConsistancyEvent.Id);
        var consistantDoctor = _mapper.Map<Domain.Data.Models.Doctor>(request.DoctorCheckConsistancyEvent);
        if (doctor is null)
        {
            await _repositoryManager.Doctor.CreateAsync(consistantDoctor);
            _logger.Information($"Succesfully added Doctor: {consistantDoctor} !");
        }

        if (doctor is not null)
        {
            doctor = _mapper.Map(consistantDoctor, doctor);
            await _repositoryManager.Doctor.UpdateAsync(doctor.Id, doctor);
            _logger.Information($"Succesfully updated Doctor: {doctor} !");
        }
    }
}
