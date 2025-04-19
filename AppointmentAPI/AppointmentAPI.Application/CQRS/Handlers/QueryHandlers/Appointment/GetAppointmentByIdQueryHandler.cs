using AppointmentAPI.Application.CQRS.Queries.Appointment;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.Appointment;

public class GetAppointmentByIdQueryHandler : IRequestHandler<GetAppointmentByIdQuery, ResponseMessage<AppointmentInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAppointmentByIdQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<AppointmentInfoDTO>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
    {
        var appointment = await _repositoryManager.Appointment.
                GetByIdAsync(
                    request.Id, 
                    o => o.Office,
                    p => p.Patient,
                    d => d.Doctor,
                    s => s.Service, 
                    sp => sp.Specialization);
        if (appointment is null)
        {
            return new ResponseMessage<AppointmentInfoDTO>("Appointment not Found!", 404);
        }

        var apointmentInfoDTO = _mapper.Map<AppointmentInfoDTO>(appointment);

        return new ResponseMessage<AppointmentInfoDTO>(apointmentInfoDTO);
    }
}
