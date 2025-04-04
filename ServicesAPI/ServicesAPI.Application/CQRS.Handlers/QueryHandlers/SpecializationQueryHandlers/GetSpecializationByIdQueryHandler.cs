using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.SpecializationQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.SpecializationQueryHandlers;

public class GetSpecializationByIdQueryHandler : IRequestHandler<GetSpecializationByIdQuery, ResponseMessage<SpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetSpecializationByIdQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<SpecializationInfoDTO>> Handle(GetSpecializationByIdQuery request, CancellationToken cancellationToken)
    {
        var specialization = await _repositoryManager.Specialization.GetByIdAsync(request.Id);
        if(specialization is null)
        { 
            return new ResponseMessage<SpecializationInfoDTO>("Specialization not Found!", 404);
        }
        var specializationInfoDTO = _mapper.Map<SpecializationInfoDTO>(specialization);

        return new ResponseMessage<SpecializationInfoDTO>(specializationInfoDTO);
    }
}
