namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Fluent API Configuration for NationalIdCard entity
public class NationalIdCardConfiguration : IEntityTypeConfiguration<NationalIdCard>
{
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
