using AppointmentAPI.Application.CQRS.Commands.Doctor;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Doctor;

public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateDoctorCommandHandler(
            IRepositoryManager repositoryManager,
            ILogger logger,
            IMapper mapper)
    {
        _logger = logger;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task Handle(UpdateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = _mapper.Map<Domain.Data.Models.Doctor>(request.DoctorUpdatedEvent);
        if (doctor is not null)
        {
            await _repositoryManager.Doctor.UpdateAsync(request.DoctorUpdatedEvent.Id, doctor);
            await _repositoryManager.CommitAsync();
            _logger.Information($"Succesfully updated Doctor: {doctor} !");
        }
    }
}
