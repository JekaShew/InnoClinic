using AppointmentAPI.Application.CQRS.Queries.Patient;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.PatientEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Patient;

public class RequestCheckPatientsConsistancyQueryHandler : IRequestHandler<RequestCheckPatientsConsistancyQuery, ResponseMessage>
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public RequestCheckPatientsConsistancyQueryHandler(ICommonService commonService, IPublishEndpoint publishEndpoint, ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ResponseMessage> Handle(RequestCheckPatientsConsistancyQuery request, CancellationToken cancellationToken)
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }

        var patientRequestCheckConsistancyEvent = new PatientRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(patientRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check Patient Consistancy by user with ID: {patientRequestCheckConsistancyEvent.UserId} at {patientRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }
}
