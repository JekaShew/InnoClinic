using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Persistance.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Data;

public class AppointmentsDBContext(DbContextOptions<AppointmentsDBContext> options) : DbContext(options)
{
    public DbSet<Service> Services { get; set; }
    public DbSet<Specialization> Specialziations { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<DoctorSpecialization> DoctorSpecializations { get; set; }
    public DbSet<Office> Offices { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<AppointmentResult> AppointmentResults { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.GetForeignKeys()
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
        }

        modelBuilder.ApplyConfiguration(new AppointmentConfiguration());
        modelBuilder.ApplyConfiguration(new AppointmentResultConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
