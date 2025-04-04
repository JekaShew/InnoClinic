using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServicesAPI.Domain.Data.Models;

namespace ServicesAPI.Persistance.Data.Configurations;

public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.HasIndex(sg => sg.Title).IsUnique();
    }
}
