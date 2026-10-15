namespace Huwiyati.Infrastructure.Persistence;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Common;
using Huwiyati.Domain.Entities.Audit;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Entities.Requests;
using Huwiyati.Infrastructure.Identity;
using Huwiyati.Infrastructure.Persistence.Audit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

/// <summary>
/// Entity Framework Core database context for the Huwiyati application, integrating ASP.NET Core Identity with application domain entities.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
{
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
    /// </summary>
    /// <param name="options">The database context options.</param>
    /// <param name="currentUserService">The current user service to capture audit actor information.</param>
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService)
        : base(options)
    {
        _currentUserService = currentUserService;
    }

    public DbSet<AuditLog> AuditLogs { get; set; } = null!;
    public DbSet<Person> Persons { get; set; } = null!;
    public DbSet<VerificationCode> VerificationCodes { get; set; } = null!;
    public DbSet<UserDevice> UserDevices { get; set; } = null!;
    public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
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
    public DbSet<ServiceType> ServiceTypes { get; set; } = null!;
    public DbSet<ServiceRequest> ServiceRequests { get; set; } = null!;
    public DbSet<RequestStatusHistory> RequestStatusHistories { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var currentUserId = _currentUserService.UserId;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.CreatedAt == default)
                {
                    entry.Entity.CreatedAt = now;
                }

                if (string.IsNullOrEmpty(entry.Entity.CreatedBy))
                {
                    entry.Entity.CreatedBy = currentUserId;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.LastModifiedAt = now;
                entry.Entity.LastModifiedBy = currentUserId;
            }
        }

        // Capture audit log entries for changes
        var auditEntries = OnBeforeSaveChanges(currentUserId, now);
        foreach (var auditEntry in auditEntries)
        {
            AuditLogs.Add(auditEntry.ToAuditLog());
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private List<AuditEntry> OnBeforeSaveChanges(string? currentUserId, DateTime now)
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            // Only audit entities that inherit from BaseEntity
            if (entry.Entity is not BaseEntity)
                continue;

            var auditEntry = new AuditEntry(entry)
            {
                EntityName = entry.Entity.GetType().Name,
                UserId = currentUserId,
                Timestamp = now
            };

            auditEntries.Add(auditEntry);

            var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey());
            auditEntry.EntityId = primaryKey?.CurrentValue?.ToString() ?? Guid.Empty.ToString();

            switch (entry.State)
            {
                case EntityState.Added:
                    auditEntry.Action = "Create";
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.Metadata.IsPrimaryKey()) continue;
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    break;

                case EntityState.Deleted:
                    auditEntry.Action = "Delete";
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.Metadata.IsPrimaryKey()) continue;
                        auditEntry.OldValues[prop.Metadata.Name] = prop.OriginalValue;
                    }
                    break;

                case EntityState.Modified:
                    auditEntry.Action = "Update";
                    foreach (var prop in entry.Properties)
                    {
                        if (prop.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(prop.Metadata.Name);
                            auditEntry.OldValues[prop.Metadata.Name] = prop.OriginalValue;
                            auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                        }
                    }
                    break;
            }
        }

        return auditEntries;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Automatically apply all configurations from current assembly (Infrastructure)
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}