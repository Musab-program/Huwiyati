namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(o => o.IsActive)
            .IsRequired();

        // Relationship: Organization has many Branches
        builder.HasMany(o => o.Branches)
            .WithOne(b => b.Organization)
            .HasForeignKey(b => b.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}