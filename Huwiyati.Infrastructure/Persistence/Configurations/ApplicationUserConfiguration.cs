namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Infrastructure.Identity;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ApplicationUser"/> entity.
/// </summary>
public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    /// <summary>
    /// Configures the entity mapping, table name, properties, and relationships for <see cref="ApplicationUser"/>.
    /// </summary>
    /// <param name="builder">The builder to configure the entity.</param>
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Customize SQL Table Name to ApplicationUsers
        builder.ToTable("ApplicationUsers");

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        // Required 1-to-0..1 Relationship (Every ApplicationUser MUST have a Person)
        builder.HasOne(u => u.Person)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.PersonId)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
