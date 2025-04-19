using AppointmentAPI.Application.CQRS.Commands.Office;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Office;

public class CheckOfficeConsistancyCommandHandler : IRequestHandler<CheckOfficeConsistancyCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public CheckOfficeConsistancyCommandHandler(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(CheckOfficeConsistancyCommand request, CancellationToken cancellationToken)
    {
        var consistantOffice = _mapper.Map<Domain.Data.Models.Office>(request.OfficeCheckConsistancyEvent);
        var office = await _repositoryManager.Office.GetByIdAsync(request.OfficeCheckConsistancyEvent.Id);
        
        if (office is null)
        {
            await _repositoryManager.Office.CreateAsync(consistantOffice);
            _logger.Information($"Succesfully added Office: {consistantOffice}");
        }

        if (office is not null)
        {
            office = _mapper.Map(consistantOffice, office);
            await _repositoryManager.Office.UpdateAsync(office.Id, office);
            _logger.Information($"Succesfully updated Office: {office}");
        }
    }
}
