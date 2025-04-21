using AppointmentAPI.Application.CQRS.Queries.Service;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.ServiceEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Service;

public class RequestCheckServicesConsistancyQueryHandler : IRequestHandler<RequestCheckServicesConsistancyQuery, ResponseMessage>
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public RequestCheckServicesConsistancyQueryHandler(
            ICommonService commonService, 
            IPublishEndpoint publishEndpoint, 
            ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ResponseMessage> Handle(RequestCheckServicesConsistancyQuery request, CancellationToken cancellationToken)
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }

        var serviceRequestCheckConsistancyEvent = new ServiceRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(serviceRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check Service Consistancy by user with ID: {serviceRequestCheckConsistancyEvent.UserId} at {serviceRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }
}
