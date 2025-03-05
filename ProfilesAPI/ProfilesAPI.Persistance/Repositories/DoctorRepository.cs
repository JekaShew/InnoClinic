using Dapper.Contrib.Extensions;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Persistance.Data;

namespace ProfilesAPI.Persistance.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ProfilesDBContext _profilesDBContext;

    public DoctorRepository(ProfilesDBContext profilesDBContext)
    {
        _profilesDBContext = profilesDBContext;
    }

    public async Task AddDoctorAsync(Doctor doctor)
    {
        await _profilesDBContext.Connection.InsertAsync<Doctor>(doctor);
    }

    public async Task DeleteDoctorByIdAsync(Guid doctorId)
    {
        await _profilesDBContext.Connection.DeleteAsync<Doctor>(new Doctor { UserId = doctorId });
    }

    public async Task<ICollection<Doctor>> GetAllDoctorsAsync()
    {
        var doctors = await _profilesDBContext.Connection.GetAllAsync<Doctor>();

        return doctors.ToList();
    }

    public async Task<Doctor> GetDoctorByIdAsync(Guid doctorId)
    {
        var doctor = await _profilesDBContext.Connection.GetAsync<Doctor>(doctorId);

        return doctor;
    }

    public async Task UpdateDoctorAsync(Doctor updatedDoctor)
    {
        await _profilesDBContext.Connection.UpdateAsync<Doctor>(updatedDoctor);
    }
}
