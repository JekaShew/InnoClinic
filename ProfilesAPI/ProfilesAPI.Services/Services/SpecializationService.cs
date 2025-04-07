using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using ProfilesAPI.Services.Abstractions.Interfaces;

namespace ProfilesAPI.Services.Services;

public class SpecializationService : ISpecializationService
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;

    public SpecializationService(
        ICommonService commonService, 
        IPublishEndpoint publishEndpoint)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ResponseMessage> RequestCheckSpecializationConsistancyAsync()
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }
        // Maybe  set security in this object IN FUTURE!
        var specializationRequestCheckConsistancyEvent = new SpecializationRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };
        await _publishEndpoint.Publish(specializationRequestCheckConsistancyEvent);

        return new ResponseMessage();
    }
}
