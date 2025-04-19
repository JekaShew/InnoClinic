using AppointmentAPI.Application.CQRS.Queries.AppointmentResult;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AppointmentAPI.Shared.DTOs.ServiceDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.AppointmentResult;

public class GetAppointmentResultByIdQueryHandler : IRequestHandler<GetAppointmentResultByIdQuery, ResponseMessage<AppointmentResultInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAppointmentResultByIdQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<AppointmentResultInfoDTO>> Handle(GetAppointmentResultByIdQuery request, CancellationToken cancellationToken)
    {
        var appointmentResult = await _repositoryManager.AppointmentResult.GetByIdAsync(request.Id, a => a.Appointment);
        if (appointmentResult is null)
        {
            return new ResponseMessage<AppointmentResultInfoDTO>("Appointment result not Found!", 404);
        }
        var appointmentResultInfoDTO = _mapper.Map<AppointmentResultInfoDTO>(appointmentResult);

        return new ResponseMessage<AppointmentResultInfoDTO>(appointmentResultInfoDTO);
    }
}
