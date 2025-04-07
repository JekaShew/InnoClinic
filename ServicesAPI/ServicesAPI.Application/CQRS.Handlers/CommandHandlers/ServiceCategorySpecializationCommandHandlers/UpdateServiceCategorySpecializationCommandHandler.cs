using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCategorySpecializationCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCategorySpecializationCommandHandlers;

public class UpdateServiceCategorySpecializationCommandHandler : IRequestHandler<UpdateServiceCategorySpecializationCommand, ResponseMessage<ServiceCategorySpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UpdateServiceCategorySpecializationCommandHandler(IRepositoryManager repositorymanager, IMapper mapper)
    {
        _repositoryManager = repositorymanager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceCategorySpecializationInfoDTO>> Handle(UpdateServiceCategorySpecializationCommand request, CancellationToken cancellationToken)
    {
        var serviceCategorySpecialization = await _repositoryManager.ServiceCategorySpecialization.GetByIdAsync(request.ServiceCategorySpecializationId);
        if (serviceCategorySpecialization is null)
        {
            return new ResponseMessage<ServiceCategorySpecializationInfoDTO>("Related Service Category and Specialization not Found!", 404);
        }

        serviceCategorySpecialization = _mapper.Map<ServiceCategorySpecialization>(request.ServiceCategorySpecializationForUpdateDTO);
        await _repositoryManager.ServiceCategorySpecialization.UpdateAsync(request.ServiceCategorySpecializationId, serviceCategorySpecialization);
        await _repositoryManager.CommitAsync();
        var serviceCategorySpecialziationInfoDTO = _mapper.Map<ServiceCategorySpecializationInfoDTO>(serviceCategorySpecialization);

        return new ResponseMessage<ServiceCategorySpecializationInfoDTO>(serviceCategorySpecialziationInfoDTO);
    }
}
