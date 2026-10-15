namespace Huwiyati.Infrastructure.Persistence.Seeders;

using Huwiyati.Domain.Entities.CivilRegistry;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Provides seeding logic for default civil and identity service types.
/// </summary>
public static class ServiceTypeSeeder
{
    /// <summary>
    /// Seeds the default citizen service types (e.g., national ID renewal, passport renewal) into the database.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task SeedServiceTypesAsync(ApplicationDbContext context)
    {
        // Fetch Organizations to associate ServiceTypes with correct authorities
        var civilRegistryOrg = await context.Organizations.FirstOrDefaultAsync(o => o.Name.Contains("الأحوال المدنية"));
        var passportsOrg = await context.Organizations.FirstOrDefaultAsync(o => o.Name.Contains("الجوازات"));

        if (civilRegistryOrg == null || passportsOrg == null)
        {
            return;
        }

        var defaultServiceTypes = new List<ServiceType>
        {
            new ServiceType
            {
                Id = Guid.Parse("018f7d9a-6000-7000-8000-000000000001"),
                Name = "تجديد بطاقة شخصية",
                Code = "NATIONAL_ID_RENEWAL",
                OrganizationId = civilRegistryOrg.Id,
                IsActive = true
            },
            new ServiceType
            {
                Id = Guid.Parse("018f7d9a-6000-7000-8000-000000000002"),
                Name = "تجديد جواز سفر",
                Code = "PASSPORT_RENEWAL",
                OrganizationId = passportsOrg.Id,
                IsActive = true
            },
            new ServiceType
            {
                Id = Guid.Parse("018f7d9a-6000-7000-8000-000000000003"),
                Name = "تجديد بطاقة عائلية",
                Code = "FAMILY_CARD_RENEWAL",
                OrganizationId = civilRegistryOrg.Id,
                IsActive = true
            },
            new ServiceType
            {
                Id = Guid.Parse("018f7d9a-6000-7000-8000-000000000004"),
                Name = "تسجيل شهادة ميلاد",
                Code = "BIRTH_CERTIFICATE_REGISTRATION",
                OrganizationId = civilRegistryOrg.Id,
                IsActive = true
            },
            new ServiceType
            {
                Id = Guid.Parse("018f7d9a-6000-7000-8000-000000000005"),
                Name = "تسجيل شهادة وفاة",
                Code = "DEATH_CERTIFICATE_REGISTRATION",
                OrganizationId = civilRegistryOrg.Id,
                IsActive = true
            }
        };

        foreach (var serviceType in defaultServiceTypes)
        {
            if (!await context.ServiceTypes.AnyAsync(st => st.Code == serviceType.Code || st.Id == serviceType.Id))
            {
                await context.ServiceTypes.AddAsync(serviceType);
            }
        }

        await context.SaveChangesAsync();
    }
}
