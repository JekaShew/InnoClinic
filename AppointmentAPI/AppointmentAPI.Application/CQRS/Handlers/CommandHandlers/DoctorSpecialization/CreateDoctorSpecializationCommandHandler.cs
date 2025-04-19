using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.DoctorSpecialization;

public class CreateDoctorSpecializationCommandHandler : IRequestHandler<CreateDoctorSpecializationCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CreateDoctorSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CreateDoctorSpecializationCommand request, CancellationToken cancellationToken)
    {
        var doctorSpecializtion = _mapper.Map<Domain.Data.Models.DoctorSpecialization>(request.DoctorSpecializationCreatedEvent);
        await _repositoryManager.DoctorSpecialization.CreateAsync(doctorSpecializtion);
        await _repositoryManager.CommitAsync();
        _logger.Information($"Succesfully added Doctor's Specialization: {doctorSpecializtion} !");
    }
}
