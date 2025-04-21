using AppointmentAPI.Application.CQRS.Queries.DoctorSpecialization;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.DoctorSpecialization;

public class RequestCheckDoctorSpecializationsConsistancyQueryHandler : IRequestHandler<RequestCheckDoctorSpecializationsConsistancyQuery, ResponseMessage>
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger _logger;

    public RequestCheckDoctorSpecializationsConsistancyQueryHandler(ICommonService commonService, IPublishEndpoint publishEndpoint, ILogger logger)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ResponseMessage> Handle(RequestCheckDoctorSpecializationsConsistancyQuery request, CancellationToken cancellationToken)
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }

        var doctorSpecializationRequestCheckConsistancyEvent = new DoctorSpecializationRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(doctorSpecializationRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check Doctor's Specialization Consistancy by user with ID: {doctorSpecializationRequestCheckConsistancyEvent.UserId} at {doctorSpecializationRequestCheckConsistancyEvent.DateTime}!");

        return new ResponseMessage();
    }
}
