using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.DoctorSpecialization;

public class UpdateDoctorSpecializationCommandHandler : IRequestHandler<UpdateDoctorSpecializationCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateDoctorSpecializationCommandHandler(
            IRepositoryManager repositoryManager,
            ILogger logger,
            IMapper mapper)
    {
        _logger = logger;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task Handle(UpdateDoctorSpecializationCommand request, CancellationToken cancellationToken)
    {
        var doctorSpecialization = _mapper.Map<Domain.Data.Models.DoctorSpecialization>(request.DoctorSpecializationUpdatedEvent);
        if (doctorSpecialization is not null)
        {
            await _repositoryManager.DoctorSpecialization.UpdateAsync(request.DoctorSpecializationUpdatedEvent.Id, doctorSpecialization);
            await _repositoryManager.CommitAsync();
            _logger.Information($"Succesfully updated DoctorSpecialization: {doctorSpecialization}");
        }
    }
}
