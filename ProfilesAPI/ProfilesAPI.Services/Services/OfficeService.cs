using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;

namespace ProfilesAPI.Services.Services;

public class OfficeService : IOfficeService
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    
    public OfficeService(
        ICommonService commonService,
        IPublishEndpoint publishEndpoint)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
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
        
        return new ResponseMessage();
    }

    //public Task<ResponseMessage> GetAllOfficesAsync()
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<ResponseMessage> GetOfficeByIdAsync()
    //{
    //    throw new NotImplementedException();
    //}
}
