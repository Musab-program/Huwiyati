namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Fluent API Configuration covering every property in Family entity
public class FamilyConfiguration : IEntityTypeConfiguration<Family>
{
    public void Configure(EntityTypeBuilder<Family> builder)
    {
        builder.ToTable("Families");

        // Primary Key
        builder.HasKey(f => f.Id);

        // Auditable Entity inherited properties
        builder.Property(f => f.CreatedAt).IsRequired();
        builder.Property(f => f.CreatedBy).HasMaxLength(100).IsRequired(false);
        builder.Property(f => f.LastModifiedAt).IsRequired(false);
        builder.Property(f => f.LastModifiedBy).HasMaxLength(100).IsRequired(false);

        // Entity declared properties
        builder.Property(f => f.FamilyNumber)
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(f => f.FamilyNumber);

        builder.Property(f => f.HeadOfFamilyPersonId).IsRequired();
        builder.Property(f => f.IssuingBranchId).IsRequired();
        builder.Property(f => f.IssueDate).IsRequired();
        builder.Property(f => f.ExpiryDate).IsRequired();
        builder.Property(f => f.QrCodePayload).HasMaxLength(1000).IsRequired(false);

        builder.Property(f => f.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(FamilyStatus.Active)
            .IsRequired();

        // Navigation Relationships
        builder.HasOne(f => f.HeadOfFamily)
            .WithMany()
            .HasForeignKey(f => f.HeadOfFamilyPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.IssuingBranch)
            .WithMany()
            .HasForeignKey(f => f.IssuingBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(f => f.FamilyMembers)
            .WithOne(m => m.Family)
            .HasForeignKey(m => m.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
