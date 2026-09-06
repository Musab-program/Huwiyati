namespace Huwiyati.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Infrastructure.Identity;
using Huwiyati.Domain.Entities.CivilRegistry;
using System.Reflection;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons => Set<Person>();


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Automatically apply all configurations from current assembly (Infrastructure)
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
