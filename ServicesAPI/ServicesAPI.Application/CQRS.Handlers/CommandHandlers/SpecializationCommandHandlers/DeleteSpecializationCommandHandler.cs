using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.SpecializationCommands;
using ServicesAPI.Domain.Data.IRepositories;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.SpecializationCommandHandlers;

public class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public DeleteSpecializationCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = await _repositoryManager.Specialization.GetByIdAsync(request.Id);
        if (specialization is null)
        {
            return new ResponseMessage("Specvialization not Found!", 404);
        }

        await _repositoryManager.Specialization.DeleteAsync(specialization);

        return new ResponseMessage();
    }
}
