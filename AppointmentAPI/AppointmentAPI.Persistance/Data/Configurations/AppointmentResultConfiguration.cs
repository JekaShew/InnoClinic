using AppointmentAPI.Domain.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Persistance.Data.Configurations;

public class AppointmentResultConfiguration : IEntityTypeConfiguration<AppointmentResult>
{
    public void Configure(EntityTypeBuilder<AppointmentResult> builder)
    {
        builder.HasIndex(a => a.Title).IsUnique();

        builder.Property(a => a.Title)
            .IsRequired();
    }
}