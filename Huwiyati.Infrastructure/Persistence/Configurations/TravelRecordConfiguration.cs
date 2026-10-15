namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Documents;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="TravelRecord"/> entity.
/// </summary>
public class TravelRecordConfiguration : IEntityTypeConfiguration<TravelRecord>
{
    /// <summary>
    /// Configures table mapping, column limits, and foreign key relationships to passports and branches for <see cref="TravelRecord"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<TravelRecord> builder)
    {
        builder.ToTable("TravelRecords");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.PassportId)
            .IsRequired();

        builder.Property(t => t.IssuingBranchId)
            .IsRequired();

        builder.Property(t => t.Country)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(t => t.EntryDate)
            .IsRequired();

        builder.Property(t => t.ExitDate)
            .IsRequired(false);

        // Relationships
        builder.HasOne(x => x.Passport)
            .WithMany()
            .HasForeignKey(x => x.PassportId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.IssuingBranch)
            .WithMany()
            .HasForeignKey(x => x.IssuingBranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
