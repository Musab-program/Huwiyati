namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="NationalIdCard"/> entity.
/// </summary>
public class NationalIdCardConfiguration : IEntityTypeConfiguration<NationalIdCard>
{
    /// <summary>
    /// Configures table mapping, column constraints, default values, and navigation relationships for <see cref="NationalIdCard"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<NationalIdCard> builder)
    {
        builder.ToTable("NationalIdCards");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.PersonId)
            .IsRequired();

        builder.Property(e => e.IssuingBranchId)
            .IsRequired();

        builder.Property(d => d.IssueDate)
            .IsRequired();

        builder.Property(d => d.ExpiryDate)
            .IsRequired();

        builder.Property(d => d.QrCodePayload)
            .IsRequired(false)
            .HasMaxLength(1000);

        builder.Property(d => d.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(NationalIdCardStatus.Active);

        // Explicitly map bidirectional navigation properties to prevent shadow foreign keys (PersonId1, OrganizationBranchId)
        builder.HasOne(d => d.Person)
            .WithMany(p => p.NationalIdCards)
            .HasForeignKey(d => d.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.OrganizationBranch)
            .WithMany(b => b.NationalIdCards)
            .HasForeignKey(d => d.IssuingBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
