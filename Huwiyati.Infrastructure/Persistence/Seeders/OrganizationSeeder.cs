namespace Huwiyati.Infrastructure.Persistence.Seeders;

using Huwiyati.Domain.Entities.Organizations;
using Microsoft.EntityFrameworkCore;

public static class OrganizationSeeder
{
    public static async Task SeedOrganizationsAsync(ApplicationDbContext context)
    {
        var orgCivilRegistryId = Guid.Parse("018F7D9A-0000-7000-8000-000000000010");
        var orgPassportsId     = Guid.Parse("018F7D9A-0000-7000-8000-000000000020");
        var orgTrafficId       = Guid.Parse("018F7D9A-0000-7000-8000-000000000030");
        var orgHospitalsId     = Guid.Parse("018F7D9A-0000-7000-8000-000000000040");

        var seedOrganizations = new List<Organization>
        {
            new Organization { Id = orgCivilRegistryId, Name = "الأحوال المدنية", IsActive = true },
            new Organization { Id = orgPassportsId,     Name = "الجوازات والهجرة", IsActive = true },
            new Organization { Id = orgTrafficId,       Name = "المرور", IsActive = true },
            new Organization { Id = orgHospitalsId,     Name = "المستشفيات", IsActive = true }
        };

        foreach (var org in seedOrganizations)
        {
            if (!await context.Organizations.AnyAsync(o => o.Id == org.Id))
            {
                await context.Organizations.AddAsync(org);
            }
        }

        await context.SaveChangesAsync();
    }
}
