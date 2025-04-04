using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.SpecializationCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.SpecializationCommandHandlers;

public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, ResponseMessage<SpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UpdateSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<SpecializationInfoDTO>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialiazation = await _repositoryManager.Specialization.GetByIdAsync(request.SpecializationId);
        if (specialiazation is null)
        {
            return new ResponseMessage<SpecializationInfoDTO>("Specialization not Found!", 404);
        }

        specialiazation = _mapper.Map<Specialization>(request.specializationForUpdateDTO);
        await _repositoryManager.Specialization.UpdateAsync(request.SpecializationId, specialiazation);
        await _repositoryManager.CommitAsync();
        var specializationInfoDTO = _mapper.Map<SpecializationInfoDTO>(specialiazation);

        return new ResponseMessage<SpecializationInfoDTO>(specializationInfoDTO);
    }
}
