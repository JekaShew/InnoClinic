using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;
using ServicesAPI.Domain.Data.IRepositories;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCategoryCommandHandlers;

public class DeleteServiceCategoryCommandHandler : IRequestHandler<DeleteServiceCategoryCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeleteServiceCategoryCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ResponseMessage> Handle(DeleteServiceCategoryCommand request, CancellationToken cancellationToken)
    {
        var serviceCategory = await _repositoryManager.ServiceCategory.GetByIdAsync(request.Id);
        if (serviceCategory is null)
        {
            return new ResponseMessage("Service Category not Found!", 404);
        }

        await _repositoryManager.BeginAsync();
        // Delete all ServiceCategorySpecializations related to this service category
        var serviceCategorySpecializations = await _repositoryManager.ServiceCategorySpecialization
            .GetAllByExpressionAsync(scs => scs.ServiceCategoryId.Equals(request.Id));

        foreach (var serviceCategorySpecialization in serviceCategorySpecializations)
        {
            await _repositoryManager.ServiceCategorySpecialization.DeleteAsync(serviceCategorySpecialization);
        }

        await _repositoryManager.ServiceCategory.DeleteAsync(serviceCategory);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }
}
