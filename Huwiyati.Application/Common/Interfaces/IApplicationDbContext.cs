namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Entities.Audit;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Defines the database context abstraction for the application layer.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets or sets the database set for comprehensive audit logs.
    /// </summary>
    DbSet<AuditLog> AuditLogs { get; set; }

    /// <summary>
    /// Gets or sets the database set for civil registry citizen profiles.
    /// </summary>
    DbSet<Person> Persons { get; set; }

    /// <summary>
    /// Gets or sets the database set for OTP and verification codes.
    /// </summary>
    DbSet<VerificationCode> VerificationCodes { get; set; }

    /// <summary>
    /// Gets or sets the database set for registered user devices.
    /// </summary>
    DbSet<UserDevice> UserDevices { get; set; }

    /// <summary>
    /// Gets or sets the database set for JWT refresh tokens.
    /// </summary>
    DbSet<RefreshToken> RefreshTokens { get; set; }

    /// <summary>
    /// Gets or sets the database set for governmental organizations and authorities.
    /// </summary>
    DbSet<Organization> Organizations { get; set; }

    /// <summary>
    /// Gets or sets the database set for organizational branches and centers.
    /// </summary>
    DbSet<OrganizationBranch> OrganizationBranches { get; set; }

    /// <summary>
    /// Gets or sets the database set for staff and administrative employees.
    /// </summary>
    DbSet<Employee> Employees { get; set; }

    /// <summary>
    /// Gets or sets the database set for issued national identification cards.
    /// </summary>
    DbSet<NationalIdCard> NationalIdCards { get; set; }

    /// <summary>
    /// Gets or sets the database set for official birth certificates.
    /// </summary>
    DbSet<BirthCertificate> BirthCertificates { get; set; }

    /// <summary>
    /// Gets or sets the database set for official death certificates.
    /// </summary>
    DbSet<DeathCertificate> DeathCertificates { get; set; }

    /// <summary>
    /// Gets or sets the database set for family registry records and cards.
    /// </summary>
    DbSet<Family> Families { get; set; }

    /// <summary>
    /// Gets or sets the database set for individual family member records.
    /// </summary>
    DbSet<FamilyMember> FamilyMembers { get; set; }

    /// <summary>
    /// Gets or sets the database set for registered marriage contracts.
    /// </summary>
    DbSet<MarriageContract> MarriageContracts { get; set; }

    /// <summary>
    /// Gets or sets the database set for issued citizen passports.
    /// </summary>
    DbSet<Passport> Passports { get; set; }

    /// <summary>
    /// Gets or sets the database set for international border crossing travel records.
    /// </summary>
    DbSet<TravelRecord> TravelRecords { get; set; }

    /// <summary>
    /// Gets or sets the database set for available civil and passport service catalog types.
    /// </summary>
    DbSet<ServiceType> ServiceTypes { get; set; }

    /// <summary>
    /// Gets or sets the database set for submitted service requests.
    /// </summary>
    DbSet<ServiceRequest> ServiceRequests { get; set; }

    /// <summary>
    /// Gets or sets the database set for service request status transition logs.
    /// </summary>
    DbSet<RequestStatusHistory> RequestStatusHistories { get; set; }

    /// <summary>
    /// Gets or sets the database set for user notifications.
    /// </summary>
    DbSet<Notification> Notifications { get; set; }

    /// <summary>
    /// Saves all changes made in this context to the underlying database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the save operation.</param>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
