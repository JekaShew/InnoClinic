using AutoMapper;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesAPI.Services.Services.OfficeConsumers;

public class OfficeCheckConsistancyConsumer : IConsumer<OfficeCheckConsistancyEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public OfficeCheckConsistancyConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task Consume(ConsumeContext<OfficeCheckConsistancyEvent> context)
    {
        var office = await _repositoryManager.Office.GetByIdAsync(context.Message.Id);
        var consistantOffice = _mapper.Map<Office>(context.Message);
        if(office is null)
        {
            await _repositoryManager.Office.CreateAsync(consistantOffice);
            _logger.Information($"Succesfully added Office: {consistantOffice}");
        }

        if ( office is not null && !office.Equals(consistantOffice))
        {
            await _repositoryManager.Office.UpdateAsync(consistantOffice.Id, consistantOffice);
            _logger.Information($"Succesfully updated Office: {consistantOffice}");
        }
    }
}
