using Microsoft.EntityFrameworkCore;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Persistance.Data.Configurations;

namespace ServicesAPI.Persistance.Data;

public class ServicesDBContext(DbContextOptions<ServicesDBContext> options) : DbContext(options)
{
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceCategory> ServiceCategiories { get; set; }
    public DbSet<ServiceCategorySpecialization> ServiceCategorySpecializations { get; set; }
    public DbSet<Specialization> Specializations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            entityType.GetForeignKeys()
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
                .ToList()
                .ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
        }

        modelBuilder.ApplyConfiguration(new ServiceConfiguration());
        modelBuilder.ApplyConfiguration(new ServiceCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new SpecializationConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
