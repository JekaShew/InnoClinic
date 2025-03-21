using AutoMapper;
using CommonLibrary.RabbitMQEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesAPI.Services.Services.OfficesConsumers
{
    public class OfficeCreatedConsumer : IConsumer<OfficeCreatedEvent>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        public OfficeCreatedConsumer(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task Consume(ConsumeContext<OfficeCreatedEvent> context)
        {
            var office = _mapper.Map<Office>(context.Message);
            await _repositoryManager.Office.CreateAsync(office);
        }
    }
}
 