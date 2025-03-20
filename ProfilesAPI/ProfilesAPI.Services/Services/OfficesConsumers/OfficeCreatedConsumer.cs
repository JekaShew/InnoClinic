using AutoMapper;
using CommonLibrary.RabbitMQEvents;
using MassTransit;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        public Task Consume(ConsumeContext<OfficeCreatedEvent> context)
        {
            
        }
    }
}
