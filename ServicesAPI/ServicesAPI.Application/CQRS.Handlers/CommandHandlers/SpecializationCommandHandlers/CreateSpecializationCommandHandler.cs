using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.SpecializationCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.SpecializationCommandHandlers;

public class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand, ResponseMessage<SpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public CreateSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<SpecializationInfoDTO>> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = _mapper.Map<Specialization>(request.specializationForCreateDTO);
        await _repositoryManager.Specialization.CreateAsync(specialization);
        await _repositoryManager.CommitAsync();
        var specializationInfoDTO = _mapper.Map<SpecializationInfoDTO>(specialization);

        return new ResponseMessage<SpecializationInfoDTO>(specializationInfoDTO);
    }
}
