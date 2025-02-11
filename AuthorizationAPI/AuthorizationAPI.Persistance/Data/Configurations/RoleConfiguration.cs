using AuthorizationAPI.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationAPI.Persistance.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(60);

        builder.HasData(
            new Role
            {
                Id = new Guid("73B795D3-4917-4219-A1A0-044FCC6606EA"),
                Title = "Administrator",
                Description = "The role Administrator gives full admin rights.",
            },
            new Role
            {
                Id = new Guid("0EEC148A-43D6-4B32-AFB6-1ECF3341BE6D"),
                Title = "Doctor",
                Description = "The role Doctor gives some administrative rights.",
            },
            new Role
            {
                Id = new Guid("78B25FDF-7199-4066-B677-5BC465BC3D1A"),
                Title = "Patient",
                Description = "The role Patient gives small client rigts.",
            }
        );
    }
}
