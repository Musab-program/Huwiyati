namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Infrastructure.Identity;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // Customize SQL Table Name to ApplicationUsers
        builder.ToTable("ApplicationUsers");

        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        // 1-to-0..1 Unidirectional Relationship between ApplicationUser and Person
        builder.HasOne(u => u.Person)
            .WithOne()
            .HasForeignKey<ApplicationUser>(u => u.PersonId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
