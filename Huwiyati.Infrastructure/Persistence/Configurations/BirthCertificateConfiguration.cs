namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class BirthCertificateConfiguration : IEntityTypeConfiguration<BirthCertificate>
{
    public void Configure(EntityTypeBuilder<BirthCertificate> builder)
    {
        builder.ToTable("BirthCertificates");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ChildPersonId)
            .IsRequired();

        builder.Property(b => b.FatherPersonId)
            .IsRequired();

        builder.Property(b => b.MotherPersonId)
            .IsRequired();

        builder.Property(b => b.HospitalBranchId)
            .IsRequired();

        builder.Property(b => b.CertificateNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(b => b.IssueDate)
            .IsRequired();

        // Unique Index: Certificate Number must be unique
        builder.HasIndex(b => b.CertificateNumber)
            .IsUnique();

        // Unique Index & One-to-One Relationship: A child can only have ONE birth certificate
        builder.HasIndex(b => b.ChildPersonId)
            .IsUnique();

        builder.HasOne(b => b.ChildPerson)
            .WithOne()
            .HasForeignKey<BirthCertificate>(b => b.ChildPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        // Many-to-One Relationships for Father, Mother, and Hospital Branch
        builder.HasOne(b => b.FatherPerson)
            .WithMany()
            .HasForeignKey(b => b.FatherPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.MotherPerson)
            .WithMany()
            .HasForeignKey(b => b.MotherPersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.HospitalBranch)
            .WithMany()
            .HasForeignKey(b => b.HospitalBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
