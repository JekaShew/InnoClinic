using AppointmentAPI.Application.CQRS.Queries.Appointment;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Appointment;

public class GetAllAppointmentsQueryHandler : IRequestHandler<GetAllAppointmentsQuery, ResponseMessage<IEnumerable<AppointmentTableInfoDTO>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAllAppointmentsQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<IEnumerable<AppointmentTableInfoDTO>>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
    {
        var appointments = await _repositoryManager.Appointment.GetAllWithParametersAsync(request.AppointmentParameters);
        var appointmentTableInfoDTOs = _mapper.Map<IEnumerable<AppointmentTableInfoDTO>>(appointments);

        return new ResponseMessage<IEnumerable<AppointmentTableInfoDTO>>(appointmentTableInfoDTOs);
    }
}
