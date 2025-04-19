using AppointmentAPI.Application.CQRS.Commands.Specialization;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Specialization;

public class CheckSpecializationConsistancyCommandHandler : IRequestHandler<CheckSpecializationConsistancyCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckSpecializationConsistancyCommandHandler(
            IRepositoryManager repositoryManager, 
            IMapper mapper, 
            ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CheckSpecializationConsistancyCommand request, CancellationToken cancellationToken)
    {
        var specialization = await _repositoryManager.Specialization.GetByIdAsync(request.SpecializationCheckConsistancyEvent.Id);
        var consistantSpecialization = _mapper.Map<Domain.Data.Models.Specialization>(request.SpecializationCheckConsistancyEvent);
        if (specialization is null)
        {
            await _repositoryManager.Specialization.CreateAsync(consistantSpecialization);
            _logger.Information($"Succesfully added Specialization: {consistantSpecialization}");
        }

        if (specialization is not null)
        {
            specialization = _mapper.Map(consistantSpecialization, specialization);
            await _repositoryManager.Specialization.UpdateAsync(specialization.Id, specialization);
            _logger.Information($"Succesfully updated Specialization: {specialization}");
        }
    }
}
