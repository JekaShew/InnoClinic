using AppointmentAPI.Application.CQRS.Queries.Office;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Office;

public class RequestCheckOfficesConsistancyQueryHandler : IRequestHandler<RequestCheckOfficesConsistancyQuery, ResponseMessage>
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public RequestCheckOfficesConsistancyQueryHandler(
            ICommonService commonService,
            IPublishEndpoint publishEndpoint, 
            ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ResponseMessage> Handle(RequestCheckOfficesConsistancyQuery request, CancellationToken cancellationToken)
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }

        var officeRequestCheckConsistancyEvent = new OfficeRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(officeRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check Office Consistancy by user with ID: {officeRequestCheckConsistancyEvent.UserId} at {officeRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }
}
