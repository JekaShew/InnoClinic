using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using AppointmentAPI.Domain.Data.Models;

namespace AppointmentAPI.Persistance.Data.Configurations;

public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
{
    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasIndex(a => a.Title).IsUnique();

        builder.Property(a => a.Title)
            .IsRequired();
    }
}
