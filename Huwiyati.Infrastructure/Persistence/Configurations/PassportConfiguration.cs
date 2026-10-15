namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="Passport"/> entity.
/// </summary>
public class PassportConfiguration : IEntityTypeConfiguration<Passport>
{
    /// <summary>
    /// Configures table mapping, column constraints, default values, and relationships for <see cref="Passport"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<Passport> builder)
    {
        builder.ToTable("Passports");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PassportNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(p => p.PassportNumber)
            .IsUnique();

        builder.Property(p => p.PassportType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(PassportType.Regular);

        builder.Property(p => p.IssueDate)
            .IsRequired();

        builder.Property(p => p.ExpiryDate)
            .IsRequired();

        builder.Property(p => p.QrCodePayload)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(PassportStatus.Active);

        // Relationships
        builder.HasOne(p => p.Person)
            .WithMany(pe => pe.Passports)
            .HasForeignKey(p => p.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.IssuingBranch)
            .WithMany()
            .HasForeignKey(p => p.IssuingBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
