using AuthorizationAPI.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationAPI.Persistance.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.HasOne<Role>(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId);
        builder.HasOne<UserStatus>(u => u.UserStatus)
           .WithMany(us => us.Users)
           .HasForeignKey(u => u.UserStatusId);
    }
}
