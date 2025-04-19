using AppointmentAPI.Application.CQRS.Commands.Office;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Office;

public class UpdateOfficeCommandHandler : IRequestHandler<UpdateOfficeCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public UpdateOfficeCommandHandler(
            IRepositoryManager repositoryManager,
            ILogger logger,
            IMapper mapper)
    {
        _logger = logger;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task Handle(UpdateOfficeCommand request, CancellationToken cancellationToken)
    {
        var office = _mapper.Map<Domain.Data.Models.Office>(request.OfficeUpdatedEvent);
        if (office is not null)
        {
            await _repositoryManager.Office.UpdateAsync(request.OfficeUpdatedEvent.Id, office);
            await _repositoryManager.CommitAsync();
            _logger.Information($"Succesfully updated Office: {office}");
        }
    }
}
