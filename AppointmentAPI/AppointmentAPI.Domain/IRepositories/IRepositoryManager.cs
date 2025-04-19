namespace AppointmentAPI.Domain.IRepositories;

public interface IRepositoryManager
{
    ISpecializationRepository Specialization { get; }
    IServiceRepository Service { get; }
    IAppointmentRepository Appointment { get; }
    IAppointmentResultRepository AppointmentResult { get; }
    IDoctorRepository Doctor { get; }
    IPatientRepository Patient { get; }
    IDoctorSpecializationRepository DoctorSpecialization { get; }
    IOfficeRepository Office { get; }

    abstract Task CommitAsync();
    abstract Task RollbackAsync();
    abstract Task BeginAsync();
}
