using AppointmentAPI.Application.CQRS.Commands.AppointmentResult;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.AppointmentResult;

public class CreateAppointmentResultCommandHandler : IRequestHandler<CreateAppointmentResultCommand, ResponseMessage<AppointmentResultInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public CreateAppointmentResultCommandHandler(IMapper mapper, IRepositoryManager repositoryManager)
    {
        _mapper = mapper;
        _repositoryManager = repositoryManager;
    }

    public async Task<ResponseMessage<AppointmentResultInfoDTO>> Handle(CreateAppointmentResultCommand request, CancellationToken cancellationToken)
    {
        var appointmentResult = _mapper.Map<Domain.Data.Models.AppointmentResult>(request.AppointmentResultForCreateDTO);

        //create PDF
        //upload PDF to BlobStorage
        // give upload link to appointment result in DB 

        await _repositoryManager.AppointmentResult.CreateAsync(appointmentResult);
        await _repositoryManager.CommitAsync();
        var appointmentResultInfoDTO = _mapper.Map<AppointmentResultInfoDTO>(appointmentResult);
        
        return new ResponseMessage<AppointmentResultInfoDTO>(appointmentResultInfoDTO);
    }
}
