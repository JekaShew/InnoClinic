namespace ProfilesAPI.Domain.IRepositories;

public interface IRepositoryManager 
{
    IAdministratorRepository Administrator { get; }
    IPatientRepository Patient { get; }
    IDoctorRepository Doctor { get; }
    IReceptionistRepository Receptionist { get; }
    ISpecializationRepository Specialization { get; }
    IWorkStatusRepository WorkStatus { get; }
    IOfficeRepository Office { get; }
    public abstract Task BeginTransactionAsync();
    public abstract Task CommitAsync();
    public abstract Task RollbackAsync();

}
