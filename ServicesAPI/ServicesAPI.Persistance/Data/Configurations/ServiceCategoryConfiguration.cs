using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicesAPI.Domain.Data.Models;
namespace ServicesAPI.Persistance.Data.Configurations;

public class ServiceCategoryConfiguration : IEntityTypeConfiguration<ServiceCategory>
{
    public void Configure(EntityTypeBuilder<ServiceCategory> builder)
    {
        builder.HasIndex(sg => sg.Title).IsUnique();

        builder.Property(sg => sg.Title)
            .IsRequired()
            .HasMaxLength(60);
    }
}
