namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Fluent API Configuration covering every property in MarriageContract entity
public class MarriageContractConfiguration : IEntityTypeConfiguration<MarriageContract>
{
    public void Configure(EntityTypeBuilder<MarriageContract> builder)
    {
        builder.ToTable("MarriageContracts");

        // Primary Key
        builder.HasKey(m => m.Id);

        // Auditable Entity inherited properties
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.CreatedBy).HasMaxLength(100).IsRequired(false);
        builder.Property(m => m.LastModifiedAt).IsRequired(false);
        builder.Property(m => m.LastModifiedBy).HasMaxLength(100).IsRequired(false);

        // Entity declared properties
        builder.Property(m => m.ContractNumber)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasIndex(m => m.ContractNumber)
            .IsUnique();

        builder.Property(m => m.HusbandPersonId).IsRequired();
        builder.Property(m => m.WifePersonId).IsRequired();
        builder.Property(m => m.MarriageDate).IsRequired();
        builder.Property(m => m.DocumentPhotoUrl).HasMaxLength(500).IsRequired(false);

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(MarriageStatus.Active)
            .IsRequired();

        builder.Property(m => m.ApprovedByUserId).IsRequired();

        // Navigation Relationships
        builder.HasOne(m => m.HusbandPerson)
            .WithMany()
            .HasForeignKey(m => m.HusbandPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.WifePerson)
            .WithMany()
            .HasForeignKey(m => m.WifePersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
