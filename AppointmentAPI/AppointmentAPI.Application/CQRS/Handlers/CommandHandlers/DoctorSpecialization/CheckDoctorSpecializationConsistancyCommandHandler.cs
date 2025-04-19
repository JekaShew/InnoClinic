using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.DoctorSpecialization;

public class CheckDoctorSpecializationConsistancyCommandHandler : IRequestHandler<CheckDoctorSpecializationConsistancyCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckDoctorSpecializationConsistancyCommandHandler(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CheckDoctorSpecializationConsistancyCommand request, CancellationToken cancellationToken)
    {
        var doctorSpecialization = await _repositoryManager.DoctorSpecialization.GetByIdAsync(request.DoctorSpecializationCheckConsistancyEvent.Id);
        var consistantDoctorSpecialization = _mapper.Map<Domain.Data.Models.DoctorSpecialization>(request.DoctorSpecializationCheckConsistancyEvent);
        if (doctorSpecialization is null)
        {
            await _repositoryManager.DoctorSpecialization.CreateAsync(consistantDoctorSpecialization);
            _logger.Information($"Succesfully added Doctor's Specialization: {consistantDoctorSpecialization} !");
        }

        if (doctorSpecialization is not null)
        {
            doctorSpecialization = _mapper.Map(consistantDoctorSpecialization, doctorSpecialization);
            await _repositoryManager.DoctorSpecialization.UpdateAsync(doctorSpecialization.Id, doctorSpecialization);
            _logger.Information($"Succesfully updated Doctor's Specialization: {doctorSpecialization} !");
        }
    }
}
