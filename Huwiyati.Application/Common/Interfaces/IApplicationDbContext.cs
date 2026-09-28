namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Entities.Requests;
using Microsoft.EntityFrameworkCore;

public interface IApplicationDbContext
{
    DbSet<Person> Persons { get; set; }
    DbSet<VerificationCode> VerificationCodes { get; set; }
    DbSet<UserDevice> UserDevices { get; set; }
    DbSet<Organization> Organizations { get; set; }
    DbSet<OrganizationBranch> OrganizationBranches { get; set; }
    DbSet<Employee> Employees { get; set; }
    DbSet<NationalIdCard> NationalIdCards { get; set; }
    DbSet<BirthCertificate> BirthCertificates { get; set; }
    DbSet<DeathCertificate> DeathCertificates { get; set; }
    DbSet<Family> Families { get; set; }
    DbSet<FamilyMember> FamilyMembers { get; set; }
    DbSet<MarriageContract> MarriageContracts { get; set; }
    DbSet<Passport> Passports { get; set; }
    DbSet<TravelRecord> TravelRecords { get; set; }
    DbSet<ServiceType> ServiceTypes { get; set; }
    DbSet<ServiceRequest> ServiceRequests { get; set; }
    DbSet<RequestStatusHistory> RequestStatusHistories { get; set; }
    DbSet<Notification> Notifications { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
