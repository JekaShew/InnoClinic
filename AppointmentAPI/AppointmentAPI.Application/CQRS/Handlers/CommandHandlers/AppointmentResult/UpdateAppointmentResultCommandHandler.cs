using AppointmentAPI.Application.CQRS.Commands.AppointmentResult;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.AppointmentResult;

public class UpdateAppointmentResultCommandHandler : IRequestHandler<UpdateAppointmentResultCommand, ResponseMessage<AppointmentResultInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UpdateAppointmentResultCommandHandler(
            IRepositoryManager repositoryManager, 
            IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<AppointmentResultInfoDTO>> Handle(UpdateAppointmentResultCommand request, CancellationToken cancellationToken)
    {
        var appointmentResult = await _repositoryManager.AppointmentResult.GetByIdAsync(request.AppointmentResultId);
        if (appointmentResult is null)
        {
            return new ResponseMessage<AppointmentResultInfoDTO>("AppointmentResult not Found!", 404);
        }

        appointmentResult = _mapper.Map(request.AppointmentResultForUpdateDTO, appointmentResult);

        //recreate PDF
        //delete PDF from BlobStorage
        //upload new PDF to BlobStorage
        // give upload link to appointment result in DB 

        await _repositoryManager.AppointmentResult.UpdateAsync(request.AppointmentResultId, appointmentResult);
        await _repositoryManager.CommitAsync();
        var appointmentResultInfoDTO = _mapper.Map<AppointmentResultInfoDTO>(appointmentResult);

        return new ResponseMessage<AppointmentResultInfoDTO>(appointmentResultInfoDTO);
    }
}
