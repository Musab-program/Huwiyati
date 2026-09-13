namespace Huwiyati.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Infrastructure.Identity;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Authentication;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Automatically apply all configurations from current assembly (Infrastructure)
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
//dotnet ef migrations add AddVerificationCodeTable --project Huwiyati.Infrastructure --startup-project Huwiyati.API