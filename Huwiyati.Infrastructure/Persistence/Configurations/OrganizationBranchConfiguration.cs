namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="OrganizationBranch"/> entity.
/// </summary>
public class OrganizationBranchConfiguration : IEntityTypeConfiguration<OrganizationBranch>
{
    /// <summary>
    /// Configures table mapping, properties, and constraints for <see cref="OrganizationBranch"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<OrganizationBranch> builder)
    {
        builder.ToTable("OrganizationBranches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BranchName)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(b => b.Governorate)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.District)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(b => b.AddressDetails)
            .HasMaxLength(250)
            .IsRequired(false);

        builder.Property(b => b.PhoneNumber)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(b => b.IsActive)
            .IsRequired();

        builder.Property(b => b.CreatedAt)
            .IsRequired();
    }
}