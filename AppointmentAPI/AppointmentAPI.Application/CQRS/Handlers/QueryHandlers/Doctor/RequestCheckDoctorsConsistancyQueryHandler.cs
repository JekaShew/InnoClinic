using AppointmentAPI.Application.CQRS.Queries.Doctor;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.DoctorEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Doctor;

public class RequestCheckDoctorsConsistancyQueryHandler : IRequestHandler<RequestCheckDoctorsConsistancyQuery, ResponseMessage>
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public RequestCheckDoctorsConsistancyQueryHandler(ICommonService commonService, IPublishEndpoint publishEndpoint, ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ResponseMessage> Handle(RequestCheckDoctorsConsistancyQuery request, CancellationToken cancellationToken)
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }

        var docotrRequestCheckConsistancyEvent = new DoctorRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(docotrRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check Doctor Consistancy by user with ID: {docotrRequestCheckConsistancyEvent.UserId} at {docotrRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }
}
