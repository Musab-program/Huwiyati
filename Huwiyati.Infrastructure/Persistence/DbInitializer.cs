namespace Huwiyati.Infrastructure.Persistence;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;
using Huwiyati.Domain.Constants;

using Huwiyati.Infrastructure.Identity;

public class DbInitializer
{
    public static async Task SeedAsync(
        ApplicationDbContext context,
        RoleManager<IdentityRole<Guid>> roleManager,
        UserManager<ApplicationUser> userManager)
    {
        // 1. Seed Roles
        string[] roles = new[] { AppRoles.Citizen, AppRoles.Employee, AppRoles.Admin, AppRoles.SuperAdmin };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
            }
        }

        // 2. Ensure existing users have Citizen role if no roles assigned
        var existingUsers = await userManager.Users.ToListAsync();
        foreach (var user in existingUsers)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                await userManager.AddToRoleAsync(user, AppRoles.Citizen);
            }
        }

        // 2. Seed Persons
        if (await context.Persons.AnyAsync()) return;

        var testPerson = new Person
        {
            Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000001"),
            NationalNumber = "01011135650",
            FirstName = "مصعب",
            FatherName = "محمد",
            GrandfatherName = "أحمد ناشر",
            FamilyName = "النجري",
            DateOfBirth = new DateOnly(2004, 9, 10),
            PlaceOfBirth = "صنعاء",
            Gender = Gender.Male,
            Nationality = "يمني",
            MaritalStatus = MaritalStatus.Single,
            Governorate = "أمانة العاصمة",
            District = "الثورة",
            AddressDetails = "شارع الستين",
            PersonStatus = PersonStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        await context.Persons.AddAsync(testPerson);
        await context.SaveChangesAsync();
    }
}