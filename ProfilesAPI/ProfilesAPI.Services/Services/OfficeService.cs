using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using Serilog;

namespace ProfilesAPI.Services.Services;

public class OfficeService : IOfficeService
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public OfficeService(
        ICommonService commonService,
        IPublishEndpoint publishEndpoint,
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ResponseMessage> RequestCheckOfficeConsistancyAsync()
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }
        // Maybe  set security in this object IN FUTURE!
        var officeRequestCheckConsistancyEvent = new OfficeRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(officeRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check OfficeConsistancy by user with ID: {officeRequestCheckConsistancyEvent.UserId} at {officeRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }

    public async Task CreateOfficeAsync(OfficeCreatedEvent officeCreatedEvent)
    {
        var office = _mapper.Map<Office>(officeCreatedEvent);
        await _repositoryManager.Office.CreateAsync(office);
        _logger.Information($"Succesfully added Office: {office}");
    }

    public async Task DeleteOfficeAsync(OfficeDeletedEvent officeDeletedEvent)
    {
        var officeToDelete = await _repositoryManager.Office.GetByIdAsync(officeDeletedEvent.Id);
        try
        {
            if (officeToDelete is not null)
            {
                await _repositoryManager.Office.DeleteAsync(officeToDelete);
                _logger.Information($"Succesfully deleted Office with Id: {officeDeletedEvent.Id}");
            }
            else
            {
                _logger.Information($"Error while deleting Office with Id: {officeDeletedEvent.Id}! No Such Office Found!");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Error deleting Office with Id: {officeDeletedEvent.Id}. Exception: {ex.Message}");
            _logger.Error($"UNABLE to DELETE Office with Id:{officeDeletedEvent.Id}! It's IsDelete Status Changed to TRUE! Please Delete this Office with Id: {officeDeletedEvent.Id} as soon as possible!");
            officeToDelete.IsDelete = true;
            await _repositoryManager.Office.UpdateAsync(officeToDelete.Id, officeToDelete);
            _logger.Error($"Error deleting Office with Id: {officeDeletedEvent.Id}. Exception: {ex.Message}");
        }
    }

    public async Task UpdateOfficeAsync(OfficeUpdatedEvent officeUpdatedEvent)
    {
        var office = _mapper.Map<Office>(officeUpdatedEvent);
        await _repositoryManager.Office.UpdateAsync(officeUpdatedEvent.Id, office);
        _logger.Information($"Succesfully updated Office: {office}");
    }

    public async Task CheckOfficeConsistancyAsync(OfficeCheckConsistancyEvent officeCreatedEvent)
    {

        var office = await _repositoryManager.Office.GetByIdAsync(officeCreatedEvent.Id);
        var consistantOffice = _mapper.Map<Office>(officeCreatedEvent);
        if (office is null)
        {
            await _repositoryManager.Office.CreateAsync(consistantOffice);
            _logger.Information($"Succesfully added Office: {consistantOffice}");
        }

        if (office is not null && !office.Equals(consistantOffice))
        {
            await _repositoryManager.Office.UpdateAsync(consistantOffice.Id, consistantOffice);
            _logger.Information($"Succesfully updated Office: {consistantOffice}");
        }
    }
}
