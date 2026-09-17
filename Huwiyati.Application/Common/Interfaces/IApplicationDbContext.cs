namespace Huwiyati.Application.Common.Interfaces;

using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Entities.Organizations;
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
    DbSet<Family> Families { get; set; }
    DbSet<FamilyMember> FamilyMembers { get; set; }
    DbSet<MarriageContract> MarriageContracts { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
