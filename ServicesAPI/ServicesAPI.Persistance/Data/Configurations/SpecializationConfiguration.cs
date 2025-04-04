using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicesAPI.Domain.Data.Models;

namespace ServicesAPI.Persistance.Data.Configurations;

public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.HasIndex(s => s.Title).IsUnique();

        builder.Property(s => s.Title)
            .IsRequired()
            .HasMaxLength(60);
    }
}
