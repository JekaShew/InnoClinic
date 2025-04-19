using AppointmentAPI.Domain.IRepositories;
using AppointmentAPI.Persistance.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace AppointmentAPI.Persistance.Repositories;

public class EFRepositoryManager : IRepositoryManager
{
    private AppointmentsDBContext _appointmentsDBContext;
    private IDbContextTransaction _transaction;
    private IAppointmentRepository _appointmentRepository;
    private IAppointmentResultRepository _appointmentResultRepository;
    private IOfficeRepository _officeRepository;
    private IPatientRepository _patientRepository;
    private IDoctorRepository _doctorRepository;
    private IServiceRepository _serviceRepository;
    private ISpecializationRepository _specializationrepository;
    private IDoctorSpecializationRepository _doctorSpecializationRepository;


    public EFRepositoryManager(AppointmentsDBContext appointmentsDBContext)
    {
        _appointmentsDBContext = appointmentsDBContext;
    }

    public IAppointmentRepository Appointment
    {
        get => _appointmentRepository ?? new AppointmentRepository(_appointmentsDBContext);
    }

    public IAppointmentResultRepository AppointmentResult
    {
        get => _appointmentResultRepository ?? new AppointmentResultRepository(_appointmentsDBContext);
    }

    public IOfficeRepository Office
    {
        get => _officeRepository ?? new OfficeRepository(_appointmentsDBContext);
    }

    public IPatientRepository Patient
    {
        get => _patientRepository ?? new PatientRepository(_appointmentsDBContext);
    }

    public IDoctorRepository Doctor
    {
        get => _doctorRepository ?? new DoctorRepository(_appointmentsDBContext);
    }

    public IServiceRepository Service
    {
        get => _serviceRepository ?? new ServiceRepository(_appointmentsDBContext);
    }
   

    public ISpecializationRepository Specialization
    {
        get => _specializationrepository ?? new SpecializationRepository(_appointmentsDBContext);
    }

    public IDoctorSpecializationRepository DoctorSpecialization
    {
        get => _doctorSpecializationRepository ?? new DoctorSpecializationRepository(_appointmentsDBContext);
    }

    public async Task BeginAsync()
    {
        _transaction = _transaction ?? await _appointmentsDBContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _appointmentsDBContext.SaveChangesAsync();
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            await RollbackAsync();
            throw new Exception(ex.Message, ex.InnerException);
        }
        finally
        {
            if (_transaction is not null)
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
}

