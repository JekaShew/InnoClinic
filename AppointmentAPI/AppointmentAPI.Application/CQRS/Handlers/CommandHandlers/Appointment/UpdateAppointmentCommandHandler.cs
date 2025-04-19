using AppointmentAPI.Application.CQRS.Commands.Appointment;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Appointment;

public class UpdateAppointmentCommandHandler : IRequestHandler<UpdateAppointmentCommand, ResponseMessage<AppointmentInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UpdateAppointmentCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<AppointmentInfoDTO>> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repositoryManager.Appointment.GetByIdAsync(request.AppointmentId);
        if (appointment is null)
        {
            return new ResponseMessage<AppointmentInfoDTO>("Appointment not Found!", 404);
        }

        appointment = _mapper.Map(request.AppointmentForUpdateDTO, appointment);
        await _repositoryManager.Appointment.UpdateAsync(request.AppointmentId, appointment);
        await _repositoryManager.CommitAsync();
        var appointmentInfoDTO = _mapper.Map<AppointmentInfoDTO>(appointment);

        return new ResponseMessage<AppointmentInfoDTO>(appointmentInfoDTO);
    }
}
