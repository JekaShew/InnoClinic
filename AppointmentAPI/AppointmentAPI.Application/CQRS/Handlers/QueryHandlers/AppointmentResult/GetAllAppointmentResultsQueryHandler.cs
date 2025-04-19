using AppointmentAPI.Application.CQRS.Queries.AppointmentResult;
using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.QueryHandlers.AppointmentResult;

public class GetAllAppointmentResultsQueryHandler : IRequestHandler<GetAllAppointmentResultsQuery, ResponseMessage<IEnumerable<AppointmentResultTableInfoDTO>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAllAppointmentResultsQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<IEnumerable<AppointmentResultTableInfoDTO>>> Handle(GetAllAppointmentResultsQuery request, CancellationToken cancellationToken)
    {
        var appointmentResults = await _repositoryManager.AppointmentResult.GetAllWithParametersAsync(request.AppointmentResultParameters);
        var appointmentResultTableInfoDTOs = _mapper.Map<IEnumerable<AppointmentResultTableInfoDTO>>(appointmentResults);

        return new ResponseMessage<IEnumerable<AppointmentResultTableInfoDTO>>(appointmentResultTableInfoDTOs);
    }
}
