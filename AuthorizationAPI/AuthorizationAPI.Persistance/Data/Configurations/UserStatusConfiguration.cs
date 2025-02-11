using AuthorizationAPI.Domain.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthorizationAPI.Persistance.Data.Configurations;

public class UserStatusConfiguration : IEntityTypeConfiguration<UserStatus>
{
    public void Configure(EntityTypeBuilder<UserStatus> builder)
    {
        builder.Property(r => r.Title)
            .IsRequired()
            .HasMaxLength(60);

        builder.HasData(
            new UserStatus
            {
                Id = new Guid("B9F67CF2-60DE-48EB-82D0-8A5D6CDE1B0F"),
                Title = "Activated",
                Description = "The Activated user status means that user has been already activated.",
            },
            new UserStatus
            {
                Id = new Guid("A780B7F4-3C8B-4452-A426-E7ABC1A46949"),
                Title = "Non-Activated",
                Description = "The Non-Activated user status means that user hasn't been activated yet.",
            },
            new UserStatus
            {
                Id = new Guid("6C6FEEBA-0919-4266-B2D1-9F5B724DB31A"),
                Title = "Deleted",
                Description = "The Deleted user status means that User Deleted their account.",
            },
            new UserStatus
            {
                Id = new Guid("7B31946C-6D14-44DC-9F93-3A4C06DB902E"),
                Title = "Banned",
                Description = "The Banned user status means that User was Banned by Administrator for some action.",
            }
        );
    }
}
