namespace Huwiyati.Application.Common.Interfaces;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Authentication;

public interface IApplicationDbContext
{
    DbSet<Person> Persons { get; set; }
    DbSet<VerificationCode> VerificationCodes { get; set; }
    DbSet<UserDevice> UserDevices { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
