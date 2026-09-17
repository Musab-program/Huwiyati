namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Fluent API Configuration covering every property in FamilyMember entity
public class FamilyMemberConfiguration : IEntityTypeConfiguration<FamilyMember>
{
    public void Configure(EntityTypeBuilder<FamilyMember> builder)
    {
        builder.ToTable("FamilyMembers");

        // Primary Key
        builder.HasKey(m => m.Id);

        // Auditable Entity inherited properties
        builder.Property(m => m.CreatedAt).IsRequired();
        builder.Property(m => m.CreatedBy).HasMaxLength(100).IsRequired(false);
        builder.Property(m => m.LastModifiedAt).IsRequired(false);
        builder.Property(m => m.LastModifiedBy).HasMaxLength(100).IsRequired(false);

        // Entity declared properties
        builder.Property(m => m.FamilyId).IsRequired();
        builder.Property(m => m.PersonId).IsRequired();
        builder.Property(m => m.MarriageContractId).IsRequired(false);

        builder.Property(m => m.RelationshipType)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(FamilyMemberStatus.Active)
            .IsRequired();

        builder.Property(m => m.JoinedAt).IsRequired();
        builder.Property(m => m.LeftAt).IsRequired(false);

        // Navigation Relationships
        builder.HasOne(m => m.Family)
            .WithMany(f => f.FamilyMembers)
            .HasForeignKey(m => m.FamilyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.Person)
            .WithMany()
            .HasForeignKey(m => m.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.MarriageContract)
            .WithMany()
            .HasForeignKey(m => m.MarriageContractId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
