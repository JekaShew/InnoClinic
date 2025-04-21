using AppointmentAPI.Application.CQRS.Queries.Specialization;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Specialization;

public class RequestCheckSpecializationsConsistancyQueryHandler : IRequestHandler<RequestCheckSpecializationsConsistancyQuery, ResponseMessage>
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public RequestCheckSpecializationsConsistancyQueryHandler(
            ICommonService commonService,
            IPublishEndpoint publishEndpoint, 
            ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ResponseMessage> Handle(RequestCheckSpecializationsConsistancyQuery request, CancellationToken cancellationToken)
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }

        var specializationRequestCheckConsistancyEvent = new  SpecializationRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(specializationRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check Specialization Consistancy by user with ID: {specializationRequestCheckConsistancyEvent.UserId} at {specializationRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }
}
