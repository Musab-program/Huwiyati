namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.CivilRegistry;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
{
    public void Configure(EntityTypeBuilder<ServiceType> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.Code)
            .IsUnique();

        builder.HasOne(x => x.Organization)
            .WithMany()
            .HasForeignKey(x => x.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
