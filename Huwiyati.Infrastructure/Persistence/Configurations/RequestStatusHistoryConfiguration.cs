namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class RequestStatusHistoryConfiguration : IEntityTypeConfiguration<RequestStatusHistory>
{
    public void Configure(EntityTypeBuilder<RequestStatusHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ServiceRequest)
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
