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

        await _repositoryManager.ServiceCategory.DeleteAsync(serviceCategory);

        return new ResponseMessage();
    }
}
