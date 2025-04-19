using AppointmentAPI.Application.CQRS.Commands.Specialization;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Specialization;

public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateSpecializationCommandHandler(
            IRepositoryManager repositoryManager,
            ILogger logger, 
            IMapper mapper)
    {
        _logger = logger;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = _mapper.Map<Domain.Data.Models.Specialization>(request.SpecializationUpdatedEvent);
        if (specialization is not null)
        {
            await _repositoryManager.Specialization.UpdateAsync(request.SpecializationUpdatedEvent.Id, specialization);
            await _repositoryManager.CommitAsync();
            _logger.Information($"Succesfully updated Specialization: {specialization}");
        }
    }
}
