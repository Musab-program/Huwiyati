namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Domain.Entities.Authentication;

/// <summary>
/// Entity Framework Core configuration for the <see cref="VerificationCode"/> entity.
/// </summary>
public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
    /// <summary>
    /// Configures table mapping, column constraints, expiration timestamps, and usage tracking for <see cref="VerificationCode"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<VerificationCode> builder)
    {
        builder.ToTable("VerificationCodes");

        builder.HasKey(vc => vc.Id);

        builder.Property(vc => vc.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(vc => vc.ExpirationTime)
            .IsRequired();

        builder.Property(vc => vc.IsUsed)
            .IsRequired();

        builder.Property(vc => vc.Attempts)
            .IsRequired();

        builder.Property(vc => vc.CreatedAt)
            .IsRequired();
    }
}