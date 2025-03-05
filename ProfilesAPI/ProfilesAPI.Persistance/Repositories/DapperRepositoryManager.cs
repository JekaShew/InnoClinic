using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class DapperRepositoryManager : IRepositoryManager
{
    private ProfilesDBContext _profilesDBContext;
    private IAdministratorRepository _administratorRepository;
    private IPatientRepository _patientRepository;
    private IDoctorRepository _doctorRepository;
    private IReceptionistRepository _receptionistRepository;
    private ISpecializationRepository _specializationRepository;
    private IWorkStatusRepository _workStatusRepository;

    public DapperRepositoryManager(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public IAdministratorRepository Administrator
    {
        get => _administratorRepository ?? new AdministratorRepository(_profilesDBContext);
    }
    public IPatientRepository Patient
    {
        get => _patientRepository ?? new PatientRepository(_profilesDBContext);
    }
    public IDoctorRepository Doctor
    {
        get => _doctorRepository ?? new DoctorRepository(_profilesDBContext);
    }
    public IReceptionistRepository Receptionist
    {
        get => _receptionistRepository ?? new ReceptionistRepository(_profilesDBContext);
    }
    public ISpecializationRepository Specialization
    {
        get => _specializationRepository ?? new SpecializationRepository(_profilesDBContext);
    }
    public IWorkStatusRepository WorkStatus
    {
        get => _workStatusRepository ?? new WorkStatusRepository(_profilesDBContext);
    }

    public async Task BeginTransactionAsync()
    {
        await Task.Run(() =>
        {
            _profilesDBContext.Connection?.Open();
            _profilesDBContext.Transaction = _profilesDBContext.Connection?.BeginTransaction();
        });
    }

    public async Task CommitAsync()
    {
       await Task.Run(() =>
        {
            _profilesDBContext.Transaction?.Commit();
            _profilesDBContext.Connection?.Close();
            _profilesDBContext.Transaction?.Dispose();
            _profilesDBContext.Transaction = null;
        });
    }

    public async Task RollbackAsync()
    {
        await Task.Run(() =>
        {
            _profilesDBContext.Transaction?.Rollback();
            _profilesDBContext.Connection?.Close();
            _profilesDBContext.Transaction?.Dispose();
            _profilesDBContext.Transaction = null;
        });
    }
}
