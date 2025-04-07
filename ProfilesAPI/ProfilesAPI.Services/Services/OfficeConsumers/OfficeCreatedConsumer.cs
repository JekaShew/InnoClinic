using AutoMapper;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesAPI.Services.Services.OfficeConsumers
{
    public class OfficeCreatedConsumer : IConsumer<OfficeCreatedEvent>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        public OfficeCreatedConsumer(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OfficeCreatedEvent> context)
        {
            var office = _mapper.Map<Office>(context.Message);
            await _repositoryManager.Office.CreateAsync(office);
            _logger.Information($"Succesfully added Office: {office}");
        }
    }
}
 