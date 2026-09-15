namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrganizationBranchConfiguration : IEntityTypeConfiguration<OrganizationBranch>
{
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