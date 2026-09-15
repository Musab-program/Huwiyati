namespace Huwiyati.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Infrastructure.Identity;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.BranchId)
            .IsRequired();

        builder.Property(e => e.EmployeeNumber)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasIndex(e => e.EmployeeNumber)
            .IsUnique();

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // Relationship: Employee belongs to one OrganizationBranch (linked to b.Employees collection)
        builder.HasOne(e => e.Branch)
            .WithMany(b => b.Employees)
            .HasForeignKey(e => e.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Employee belongs to one ApplicationUser (linked to u.Employee property)
        builder.HasOne<ApplicationUser>()
            .WithOne(u => u.Employee)
            .HasForeignKey<Employee>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
