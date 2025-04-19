using AppointmentAPI.Application.CQRS.Commands.DoctorSpecialization;
using AppointmentAPI.Domain.IRepositories;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.DoctorSpecialization
{
    internal class DeleteDoctorSpecializationCommandHandler : IRequestHandler<DeleteDoctorSpecializationCommand>
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly ILogger _logger;

        public DeleteDoctorSpecializationCommandHandler(IRepositoryManager repositoryManager, ILogger logger)
        {
            _repositoryManager = repositoryManager;
            _logger = logger;
        }

        public async Task Handle(DeleteDoctorSpecializationCommand request, CancellationToken cancellationToken)
        {
            var doctorSpecializationToDelete = await _repositoryManager.DoctorSpecialization.GetByIdAsync(request.Id);

            if (doctorSpecializationToDelete is not null)
            {
                await _repositoryManager.DoctorSpecialization.DeleteAsync(doctorSpecializationToDelete);
                await _repositoryManager.CommitAsync();
                _logger.Information($"Succesfully deleted Doctor's Specialization with Id: {request.Id} !");
            }
        }
    }
}
