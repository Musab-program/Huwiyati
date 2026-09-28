namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ServiceRequestConfiguration : IEntityTypeConfiguration<ServiceRequest>
{
    public void Configure(EntityTypeBuilder<ServiceRequest> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RequestNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(x => x.RequestNumber)
            .IsUnique();

        builder.Property(x => x.BranchId)
            .IsRequired();

        builder.HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.ServiceType)
            .WithMany(x => x.ServiceRequests)
            .HasForeignKey(x => x.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Branch)
            .WithMany()
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
