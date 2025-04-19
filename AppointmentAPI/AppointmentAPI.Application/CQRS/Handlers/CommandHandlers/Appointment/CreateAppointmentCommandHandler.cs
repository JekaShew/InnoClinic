using AppointmentAPI.Application.CQRS.Commands.Appointment;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Appointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, ResponseMessage<AppointmentInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public CreateAppointmentCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<AppointmentInfoDTO>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = _mapper.Map<Domain.Data.Models.Appointment>(request.AppointmentForCreateDTO);
        await _repositoryManager.Appointment.CreateAsync(appointment);
        await _repositoryManager.CommitAsync();
        var appointmentInfoDTO = _mapper.Map<AppointmentInfoDTO>(appointment);

        return new ResponseMessage<AppointmentInfoDTO>(appointmentInfoDTO);
    }
}
