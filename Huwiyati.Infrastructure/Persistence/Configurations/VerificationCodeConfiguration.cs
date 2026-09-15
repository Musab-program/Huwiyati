namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Domain.Entities.Authentication;

public class VerificationCodeConfiguration : IEntityTypeConfiguration<VerificationCode>
{
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