namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Huwiyati.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

/// <summary>
/// Entity Framework Core configuration for the <see cref="RequestStatusHistory"/> entity.
/// </summary>
public class RequestStatusHistoryConfiguration : IEntityTypeConfiguration<RequestStatusHistory>
{
    /// <summary>
    /// Configures primary keys and cascading foreign key relationships for <see cref="RequestStatusHistory"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<RequestStatusHistory> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.ServiceRequest)
            .WithMany(x => x.StatusHistory)
            .HasForeignKey(x => x.ServiceRequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
