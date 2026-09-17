namespace Huwiyati.Infrastructure.Persistence;

using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Organizations;
using Huwiyati.Domain.Enums;
using Huwiyati.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        // 2. Seed Persons
        var p1Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000001");
        var p2Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000002");
        var p3Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000003");

        if (!await context.Persons.AnyAsync(p => p.Id == p1Id))
        {
            await context.Persons.AddAsync(new Person
            {
                Id = p1Id,
                NationalNumber = "01011135650",
                FirstName = "مصعب",
                FatherName = "محمد",
                GrandfatherName = "أحمد ناشر",
                FamilyName = "النجري",
                DateOfBirth = new DateOnly(2004, 9, 10),
                PlaceOfBirth = "صنعاء",
                BloodGroup = BloodGroup.OPositive,
                Gender = Gender.Male,
                Nationality = "يمني",
                MaritalStatus = MaritalStatus.Single,
                Governorate = "أمانة العاصمة",
                District = "الثورة",
                AddressDetails = "شارع الستين",
                PersonStatus = PersonStatus.Active,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Persons.AnyAsync(p => p.Id == p2Id))
        {
            await context.Persons.AddAsync(new Person
            {
                Id = p2Id,
                NationalNumber = "01011131317",
                FirstName = "ضياء",
                FatherName = "محمد",
                GrandfatherName = "عبدالمجيد",
                FamilyName = "السالمي",
                DateOfBirth = new DateOnly(2005, 2, 2),
                PlaceOfBirth = "التحرير - أمانة العاصمة",
                BloodGroup = BloodGroup.OPositive,
                Gender = Gender.Male,
                Nationality = "يمني",
                MaritalStatus = MaritalStatus.Single,
                Governorate = "أمانة العاصمة",
                District = "التحرير",
                AddressDetails = "التحرير",
                PersonStatus = PersonStatus.Active,
                CreatedAt = DateTime.UtcNow
            });
        }

        if (!await context.Persons.AnyAsync(p => p.Id == p3Id))
        {
            await context.Persons.AddAsync(new Person
            {
                Id = p3Id,
                NationalNumber = "01011108594",
                FirstName = "نجم الدين",
                FatherName = "يحيى",
                GrandfatherName = "يحيى محمد",
                FamilyName = "الوتاري",
                DateOfBirth = new DateOnly(2002, 3, 2),
                PlaceOfBirth = "الرجم - المحويت",
                BloodGroup = BloodGroup.OPositive,
                Gender = Gender.Male,
                Nationality = "يمني",
                MaritalStatus = MaritalStatus.Single,
                Governorate = "المحويت",
                District = "الرجم",
                AddressDetails = "الرجم",
                PersonStatus = PersonStatus.Active,
                CreatedAt = DateTime.UtcNow
            });
        }

        await context.SaveChangesAsync();

        // 3. Seed ApplicationUsers, Assign Roles, Update Phone Numbers, and Add Trusted Devices
        var seedUsers = new[]
        {
            new { PersonId = p1Id, NationalNumber = "01011135650", Email = "musabalnagri@gmail.com", PhoneNumber = "770000001", Role = AppRoles.SuperAdmin },
            new { PersonId = p2Id, NationalNumber = "01011131317", Email = "dhia.alsalmi@example.com", PhoneNumber = "770000002", Role = AppRoles.Employee },
            new { PersonId = p3Id, NationalNumber = "01011108594", Email = "najm.alwatari@example.com", PhoneNumber = "770000003", Role = AppRoles.Admin },
        };

        foreach (var uData in seedUsers)
        {
            var user = await userManager.FindByNameAsync(uData.NationalNumber);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    PersonId = uData.PersonId,
                    UserName = uData.NationalNumber,
                    Email = uData.Email,
                    PhoneNumber = uData.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    Status = AccountStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    ActivatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(user, "Password123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, uData.Role);
                }
            }
            else
            {
                // Ensure Phone Number is updated if missing
                if (string.IsNullOrEmpty(user.PhoneNumber) || user.PhoneNumber != uData.PhoneNumber)
                {
                    user.PhoneNumber = uData.PhoneNumber;
                    user.PhoneNumberConfirmed = true;
                    await userManager.UpdateAsync(user);
                }

                var currentRoles = await userManager.GetRolesAsync(user);
                if (!currentRoles.Contains(uData.Role))
                {
                    await userManager.AddToRoleAsync(user, uData.Role);
                }
            }

            // Seed Trusted Device for the user if not exists
            if (user != null)
            {
                var deviceIdentifier = $"test-trusted-device-{uData.NationalNumber}";
                var existingDevice = await context.UserDevices.FirstOrDefaultAsync(d => d.UserId == user.Id && d.DeviceIdentifier == deviceIdentifier);
                if (existingDevice == null)
                {
                    await context.UserDevices.AddAsync(new UserDevice
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        DeviceName = "Test Mobile Device",
                        DeviceIdentifier = deviceIdentifier,
                        OperatingSystem = "Android",
                        IsTrusted = true,
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else if (!existingDevice.IsTrusted)
                {
                    existingDevice.IsTrusted = true;
                    existingDevice.LastLogin = DateTime.UtcNow;
                }
            }
        }

        await context.SaveChangesAsync();

        // 4. Ensure existing users without roles get Citizen role
        var existingUsers = await userManager.Users.ToListAsync();
        foreach (var user in existingUsers)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Any())
            {
                await userManager.AddToRoleAsync(user, AppRoles.Citizen);
            }
        }

        // 5. Seed Organizations
        if (!await context.Organizations.AnyAsync())
        {
            var defaultOrganizations = new List<Organization>
            {
                new Organization { Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000010"), Name = "الأحوال المدنية", IsActive = true },
                new Organization { Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000020"), Name = "الجوازات والهجرة", IsActive = true },
                new Organization { Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000030"), Name = "المرور", IsActive = true },
                new Organization { Id = Guid.Parse("018f7d9a-0000-7000-8000-000000000040"), Name = "المستشفيات", IsActive = true }
            };

            await context.Organizations.AddRangeAsync(defaultOrganizations);
            await context.SaveChangesAsync();
        }
    }
}