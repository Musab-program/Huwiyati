namespace Huwiyati.Infrastructure.Persistence;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
    public DbSet<UserDevice> UserDevices { get; set; } = null!;
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<OrganizationBranch> OrganizationBranches { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<NationalIdCard> NationalIdCards { get; set; } = null!;
    public DbSet<BirthCertificate> BirthCertificates { get; set; } = null!;
    public DbSet<DeathCertificate> DeathCertificates { get; set; } = null!;
    public DbSet<Family> Families { get; set; } = null!;
    public DbSet<FamilyMember> FamilyMembers { get; set; } = null!;
    public DbSet<MarriageContract> MarriageContracts { get; set; } = null!;
    public DbSet<Passport> Passports { get; set; } = null!;
    public DbSet<TravelRecord> TravelRecords { get; set; } = null!;

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.CreatedAt == default)
                {
                    entry.Entity.CreatedAt = now;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedAt = now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Automatically apply all configurations from current assembly (Infrastructure)
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}