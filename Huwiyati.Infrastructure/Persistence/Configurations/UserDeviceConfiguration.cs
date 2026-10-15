namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Infrastructure.Identity;

/// <summary>
/// Entity Framework Core configuration for the <see cref="UserDevice"/> entity.
/// </summary>
public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
    /// <summary>
    /// Configures table mapping, column constraints, default values, and cascading relationships for <see cref="UserDevice"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<UserDevice> builder)
    {
        builder.ToTable("UserDevices");

        builder.HasKey(ud => ud.Id);

        builder.Property(ud => ud.DeviceName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ud => ud.DeviceIdentifier)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(ud => ud.OperatingSystem)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(ud => ud.IsTrusted)
            .IsRequired()
            .HasDefaultValue(false);

        // One-to-Many Relationship: ApplicationUser has many UserDevices
        builder.HasOne<ApplicationUser>()
            .WithMany(u => u.Devices)
            .HasForeignKey(ud => ud.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}