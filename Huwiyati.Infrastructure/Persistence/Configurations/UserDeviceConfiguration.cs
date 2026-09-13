namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Infrastructure.Identity;

public class UserDeviceConfiguration : IEntityTypeConfiguration<UserDevice>
{
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