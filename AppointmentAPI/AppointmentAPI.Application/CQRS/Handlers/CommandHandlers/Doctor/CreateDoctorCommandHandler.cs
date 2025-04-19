using AppointmentAPI.Application.CQRS.Commands.Doctor;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Doctor;

public class CreateDoctorCommandHandler : IRequestHandler<CreateDoctorCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateDoctorCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CreateDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctor = _mapper.Map<Domain.Data.Models.Doctor>(request.DoctorCreatedEvent);
        await _repositoryManager.Doctor.CreateAsync(doctor);
        await _repositoryManager.CommitAsync();
        _logger.Information($"Succesfully added Doctor: {doctor}");
    }
}
