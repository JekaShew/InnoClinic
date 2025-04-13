using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Queries.SpecializationQueries;

public class GetAllSpecializationsWithParametersQuery : IRequest<ResponseMessage<IEnumerable<SpecializationTableInfoDTO>>>
{
    public SpecializationParameters SpecializationParameters { get; set; } = new SpecializationParameters();
}
