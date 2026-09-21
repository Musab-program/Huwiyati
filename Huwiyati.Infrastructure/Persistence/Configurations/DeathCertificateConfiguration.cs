namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class DeathCertificateConfiguration : IEntityTypeConfiguration<DeathCertificate>
{
    public void Configure(EntityTypeBuilder<DeathCertificate> builder)
    {
        builder.ToTable("DeathCertificates");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.PersonId)
            .IsRequired();

        builder.Property(d => d.HospitalBranchId)
            .IsRequired();

        builder.Property(d => d.IssuingBranchId)
            .IsRequired();

        builder.Property(d => d.CertificateNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(d => d.DeathDate)
            .IsRequired();

        builder.Property(d => d.PlaceOfDeath)
            .HasMaxLength(200);

        builder.Property(d => d.CauseOfDeath)
            .HasMaxLength(500);

        builder.Property(d => d.IssueDate)
            .IsRequired();

        // Unique Index: Certificate Number must be unique
        builder.HasIndex(d => d.CertificateNumber)
            .IsUnique();

        // Unique Index & One-to-One Relationship: A person can only have ONE death certificate
        builder.HasIndex(d => d.PersonId)
            .IsUnique();

        builder.HasOne(d => d.Person)
            .WithOne()
            .HasForeignKey<DeathCertificate>(d => d.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.HospitalBranch)
            .WithMany()
            .HasForeignKey(d => d.HospitalBranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(d => d.IssuingBranch)
            .WithMany()
            .HasForeignKey(d => d.IssuingBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
