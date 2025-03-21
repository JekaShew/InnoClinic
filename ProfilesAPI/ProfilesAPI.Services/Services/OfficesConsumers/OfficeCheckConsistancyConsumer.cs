using AutoMapper;
using CommonLibrary.RabbitMQEvents;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesAPI.Services.Services.OfficesConsumers;

public class OfficeCheckConsistancyConsumer : IConsumer<OfficeCheckConsistancyEvent>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public OfficeCheckConsistancyConsumer(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<OfficeCheckConsistancyEvent> context)
    {
        var office = await _repositoryManager.Office.GetByIdAsync(context.Message.Id);
        var consistantOffice = _mapper.Map<Office>(context.Message);
        if (!office.Equals(consistantOffice))
        {
            await _repositoryManager.Office.UpdateAsync(consistantOffice.Id, consistantOffice);
        }
    }
}
