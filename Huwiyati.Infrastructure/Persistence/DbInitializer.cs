namespace Huwiyati.Infrastructure.Persistence;

using Huwiyati.Domain.Constants;
using Huwiyati.Domain.Entities.Authentication;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
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
        // -------------------------------------------------------------
        // 1. Seed Roles
        // -------------------------------------------------------------
        string[] roles = new[] { AppRoles.Citizen, AppRoles.Employee, AppRoles.Admin, AppRoles.SuperAdmin };

        foreach (var roleName in roles)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid> { Name = roleName });
            }
        }

        // -------------------------------------------------------------
        // 2. Seed Organizations and Organization Branches
        // -------------------------------------------------------------
        var orgCivilRegistryId = Guid.Parse("018f7d9a-1000-7000-8000-000000000001");
        var orgHospitalsId     = Guid.Parse("018f7d9a-1000-7000-8000-000000000002");
        var orgPassportsId     = Guid.Parse("018f7d9a-1000-7000-8000-000000000003");

        if (!await context.Organizations.AnyAsync())
        {
            var organizations = new List<Organization>
            {
                new Organization { Id = orgCivilRegistryId, Name = "الأحوال المدنية", IsActive = true },
                new Organization { Id = orgHospitalsId,     Name = "المستشفيات", IsActive = true },
                new Organization { Id = orgPassportsId,     Name = "الجوازات والهجرة", IsActive = true }
            };

            await context.Organizations.AddRangeAsync(organizations);
            await context.SaveChangesAsync();
        }

        var branchCivilRegistrySanaaId = Guid.Parse("018f7d9a-2000-7000-8000-000000000001");
        var branchCivilRegistryAdenId  = Guid.Parse("018f7d9a-2000-7000-8000-000000000002");
        var branchHospitalThawraId     = Guid.Parse("018f7d9a-2000-7000-8000-000000000003");
        var branchHospitalJumhuriId    = Guid.Parse("018f7d9a-2000-7000-8000-000000000004");

        if (!await context.OrganizationBranches.AnyAsync())
        {
            var branches = new List<OrganizationBranch>
            {
                new OrganizationBranch
                {
                    Id = branchCivilRegistrySanaaId,
                    OrganizationId = orgCivilRegistryId,
                    BranchName = "مصلحة الأحوال المدنية - صنعاء (المركز الرئيسي)",
                    Governorate = "أمانة العاصمة",
                    District = "الثورة",
                    AddressDetails = "شارع الستين الشمالي",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new OrganizationBranch
                {
                    Id = branchCivilRegistryAdenId,
                    OrganizationId = orgCivilRegistryId,
                    BranchName = "مصلحة الأحوال المدنية - عدن",
                    Governorate = "عدن",
                    District = "خور مكسر",
                    AddressDetails = "شارع الاستقلال",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new OrganizationBranch
                {
                    Id = branchHospitalThawraId,
                    OrganizationId = orgHospitalsId,
                    BranchName = "مستشفى الثورة العام",
                    Governorate = "أمانة العاصمة",
                    District = "الصافية",
                    AddressDetails = "شارع الثورة",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new OrganizationBranch
                {
                    Id = branchHospitalJumhuriId,
                    OrganizationId = orgHospitalsId,
                    BranchName = "مستشفى الجمهوري التعليمي",
                    Governorate = "أمانة العاصمة",
                    District = "التحرير",
                    AddressDetails = "شارع الزبيري",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.OrganizationBranches.AddRangeAsync(branches);
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 3. Seed Persons (Males & Females, Married & Single, Family & Independent)
        // -------------------------------------------------------------
        var pFatherId = Guid.Parse("018f7d9a-3000-7000-8000-000000000001"); // Father/Head of Family
        var pMotherId = Guid.Parse("018f7d9a-3000-7000-8000-000000000002"); // Mother/Wife
        var pSonId    = Guid.Parse("018f7d9a-3000-7000-8000-000000000003"); // Son in Family 1
        var pDauId    = Guid.Parse("018f7d9a-3000-7000-8000-000000000004"); // Daughter in Family 1
        var pSingle1Id= Guid.Parse("018f7d9a-3000-7000-8000-000000000005"); // Independent Single Male
        var pSingle2Id= Guid.Parse("018f7d9a-3000-7000-8000-000000000006"); // Independent Single Female

        if (!await context.Persons.AnyAsync(p => p.Id == pFatherId))
        {
            var persons = new List<Person>
            {
                new Person
                {
                    Id = pFatherId,
                    NationalNumber = "01001000001",
                    FirstName = "علي",
                    FatherName = "عبد الله",
                    GrandfatherName = "أحمد",
                    FamilyName = "الشامي",
                    DateOfBirth = new DateOnly(1985, 5, 15),
                    PlaceOfBirth = "صنعاء",
                    BloodGroup = BloodGroup.APositive,
                    Gender = Gender.Male,
                    Nationality = "يمني",
                    MaritalStatus = MaritalStatus.Married,
                    Governorate = "أمانة العاصمة",
                    District = "السبعين",
                    AddressDetails = "حي حدة - شارع أربيل",
                    PersonStatus = PersonStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new Person
                {
                    Id = pMotherId,
                    NationalNumber = "01001000002",
                    FirstName = "فاطمة",
                    FatherName = "محمد",
                    GrandfatherName = "حسن",
                    FamilyName = "الكبسي",
                    DateOfBirth = new DateOnly(1990, 8, 20),
                    PlaceOfBirth = "صنعاء",
                    BloodGroup = BloodGroup.OPositive,
                    Gender = Gender.Female,
                    Nationality = "يمنية",
                    MaritalStatus = MaritalStatus.Married,
                    Governorate = "أمانة العاصمة",
                    District = "السبعين",
                    AddressDetails = "حي حدة - شارع أربيل",
                    PersonStatus = PersonStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new Person
                {
                    Id = pSonId,
                    NationalNumber = "01001000003",
                    FirstName = "ياسين",
                    FatherName = "علي",
                    GrandfatherName = "عبد الله",
                    FamilyName = "الشامي",
                    DateOfBirth = new DateOnly(2020, 3, 10),
                    PlaceOfBirth = "صنعاء",
                    BloodGroup = BloodGroup.APositive,
                    Gender = Gender.Male,
                    Nationality = "يمني",
                    MaritalStatus = MaritalStatus.Single,
                    Governorate = "أمانة العاصمة",
                    District = "السبعين",
                    AddressDetails = "حي حدة - شارع أربيل",
                    PersonStatus = PersonStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new Person
                {
                    Id = pDauId,
                    NationalNumber = "01001000004",
                    FirstName = "مريم",
                    FatherName = "علي",
                    GrandfatherName = "عبد الله",
                    FamilyName = "الشامي",
                    DateOfBirth = new DateOnly(2024, 1, 1),
                    PlaceOfBirth = "صنعاء",
                    BloodGroup = BloodGroup.OPositive,
                    Gender = Gender.Female,
                    Nationality = "يمنية",
                    MaritalStatus = MaritalStatus.Single,
                    Governorate = "أمانة العاصمة",
                    District = "السبعين",
                    AddressDetails = "حي حدة - شارع أربيل",
                    PersonStatus = PersonStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new Person
                {
                    Id = pSingle1Id,
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
                },
                new Person
                {
                    Id = pSingle2Id,
                    NationalNumber = "01011131317",
                    FirstName = "سارة",
                    FatherName = "عبد المجيد",
                    GrandfatherName = "محمد",
                    FamilyName = "السالمي",
                    DateOfBirth = new DateOnly(2005, 2, 2),
                    PlaceOfBirth = "أمانة العاصمة",
                    BloodGroup = BloodGroup.BPositive,
                    Gender = Gender.Female,
                    Nationality = "يمنية",
                    MaritalStatus = MaritalStatus.Single,
                    Governorate = "أمانة العاصمة",
                    District = "التحرير",
                    AddressDetails = "شارع التحرير",
                    PersonStatus = PersonStatus.Active,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Persons.AddRangeAsync(persons);
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 4. Seed ApplicationUsers, Assign Roles & Trusted Devices
        // -------------------------------------------------------------
        var userFatherId  = Guid.Parse("018f7d9a-4000-7000-8000-000000000001");
        var userMotherId  = Guid.Parse("018f7d9a-4000-7000-8000-000000000002");
        var userSingle1Id = Guid.Parse("018f7d9a-4000-7000-8000-000000000005");
        var userEmpId     = Guid.Parse("018f7d9a-4000-7000-8000-000000000006");

        var seedUsers = new[]
        {
            new { UserId = userFatherId,  PersonId = pFatherId,  NationalNumber = "01001000001", Email = "ali.alshami@example.com", PhoneNumber = "770000001", Role = AppRoles.Citizen },
            new { UserId = userMotherId,  PersonId = pMotherId,  NationalNumber = "01001000002", Email = "fatima.alkabsi@example.com", PhoneNumber = "770000002", Role = AppRoles.Citizen },
            new { UserId = userSingle1Id, PersonId = pSingle1Id, NationalNumber = "01011135650", Email = "musabalnagri@gmail.com", PhoneNumber = "770000003", Role = AppRoles.SuperAdmin },
            new { UserId = userEmpId,     PersonId = pSingle2Id, NationalNumber = "01011131317", Email = "sara.alsalmi@example.com", PhoneNumber = "770000004", Role = AppRoles.Employee }
        };

        foreach (var uData in seedUsers)
        {
            var user = await userManager.FindByNameAsync(uData.NationalNumber);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = uData.UserId,
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

            // Seed User Device
            if (user != null)
            {
                var deviceIdentifier = $"device-{uData.NationalNumber}";
                if (!await context.UserDevices.AnyAsync(d => d.UserId == user.Id))
                {
                    await context.UserDevices.AddAsync(new UserDevice
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        DeviceName = "Mobile Device",
                        DeviceIdentifier = deviceIdentifier,
                        OperatingSystem = "Android",
                        IsTrusted = true,
                        LastLogin = DateTime.UtcNow,
                        CreatedAt = DateTime.UtcNow
                    });
                }
            }
        }

        await context.SaveChangesAsync();

        // -------------------------------------------------------------
        // 5. Seed Employee Record
        // -------------------------------------------------------------
        var employeeId = Guid.Parse("018f7d9a-5000-7000-8000-000000000001");
        if (!await context.Employees.AnyAsync(e => e.Id == employeeId))
        {
            await context.Employees.AddAsync(new Employee
            {
                Id = employeeId,
                UserId = userEmpId,
                BranchId = branchCivilRegistrySanaaId,
                EmployeeNumber = "EMP-001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 6. Seed Marriage Contract
        // -------------------------------------------------------------
        var marriageContractId = Guid.Parse("018f7d9a-6000-7000-8000-000000000001");
        if (!await context.MarriageContracts.AnyAsync(m => m.Id == marriageContractId))
        {
            await context.MarriageContracts.AddAsync(new MarriageContract
            {
                Id = marriageContractId,
                ContractNumber = "MAR-2015-0001",
                HusbandPersonId = pFatherId,
                WifePersonId = pMotherId,
                MarriageDate = new DateOnly(2015, 6, 20),
                Status = MarriageStatus.Active,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 7. Seed Family & Family Members
        // -------------------------------------------------------------
        var familyId = Guid.Parse("018f7d9a-7000-7000-8000-000000000001");
        if (!await context.Families.AnyAsync(f => f.Id == familyId))
        {
            var family = new Family
            {
                Id = familyId,
                FamilyNumber = "02001000001",
                HeadOfFamilyPersonId = pFatherId,
                IssuingBranchId = branchCivilRegistrySanaaId,
                IssueDate = new DateOnly(2015, 7, 1),
                ExpiryDate = new DateOnly(2025, 7, 1),
                Status = FamilyStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            await context.Families.AddAsync(family);
            await context.SaveChangesAsync();

            // Seed Family Members (Head, Wife, Son, Daughter)
            var familyMembers = new List<FamilyMember>
            {
                new FamilyMember
                {
                    Id = Guid.NewGuid(),
                    FamilyId = familyId,
                    PersonId = pFatherId,
                    RelationshipType = RelationshipType.Head,
                    Status = FamilyMemberStatus.Active,
                    JoinedAt = DateTime.UtcNow
                },
                new FamilyMember
                {
                    Id = Guid.NewGuid(),
                    FamilyId = familyId,
                    PersonId = pMotherId,
                    MarriageContractId = marriageContractId,
                    RelationshipType = RelationshipType.Wife,
                    Status = FamilyMemberStatus.Active,
                    JoinedAt = DateTime.UtcNow
                },
                new FamilyMember
                {
                    Id = Guid.NewGuid(),
                    FamilyId = familyId,
                    PersonId = pSonId,
                    RelationshipType = RelationshipType.Son,
                    Status = FamilyMemberStatus.Active,
                    JoinedAt = DateTime.UtcNow
                },
                new FamilyMember
                {
                    Id = Guid.NewGuid(),
                    FamilyId = familyId,
                    PersonId = pDauId,
                    RelationshipType = RelationshipType.Daughter,
                    Status = FamilyMemberStatus.Active,
                    JoinedAt = DateTime.UtcNow
                }
            };

            await context.FamilyMembers.AddRangeAsync(familyMembers);
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 8. Seed National ID Cards
        // -------------------------------------------------------------
        if (!await context.NationalIdCards.AnyAsync())
        {
            var cards = new List<NationalIdCard>
            {
                new NationalIdCard
                {
                    Id = Guid.NewGuid(),
                    PersonId = pFatherId,
                    IssuingBranchId = branchCivilRegistrySanaaId,
                    IssueDate = new DateOnly(2020, 1, 1),
                    ExpiryDate = new DateOnly(2030, 1, 1),
                    QrCodePayload = "NAT-01001000001",
                    Status = NationalIdCardStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new NationalIdCard
                {
                    Id = Guid.NewGuid(),
                    PersonId = pMotherId,
                    IssuingBranchId = branchCivilRegistrySanaaId,
                    IssueDate = new DateOnly(2021, 5, 10),
                    ExpiryDate = new DateOnly(2031, 5, 10),
                    QrCodePayload = "NAT-01001000002",
                    Status = NationalIdCardStatus.Active,
                    CreatedAt = DateTime.UtcNow
                },
                new NationalIdCard
                {
                    Id = Guid.NewGuid(),
                    PersonId = pSingle1Id,
                    IssuingBranchId = branchCivilRegistrySanaaId,
                    IssueDate = new DateOnly(2022, 9, 15),
                    ExpiryDate = new DateOnly(2032, 9, 15),
                    QrCodePayload = "NAT-01011135650",
                    Status = NationalIdCardStatus.Active,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.NationalIdCards.AddRangeAsync(cards);
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 9. Seed Birth Certificates
        // -------------------------------------------------------------
        if (!await context.BirthCertificates.AnyAsync())
        {
            var birthCertificates = new List<BirthCertificate>
            {
                new BirthCertificate
                {
                    Id = Guid.NewGuid(),
                    ChildPersonId = pSonId,
                    FatherPersonId = pFatherId,
                    MotherPersonId = pMotherId,
                    HospitalBranchId = branchHospitalThawraId,
                    CertificateNumber = "03001000001",
                    IssueDate = new DateOnly(2020, 3, 12),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userEmpId.ToString()
                },
                new BirthCertificate
                {
                    Id = Guid.NewGuid(),
                    ChildPersonId = pDauId,
                    FatherPersonId = pFatherId,
                    MotherPersonId = pMotherId,
                    HospitalBranchId = branchHospitalThawraId,
                    CertificateNumber = "03001000002",
                    IssueDate = new DateOnly(2024, 1, 3),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userEmpId.ToString()
                }
            };

            await context.BirthCertificates.AddRangeAsync(birthCertificates);
            await context.SaveChangesAsync();
        }

        // -------------------------------------------------------------
        // 10. Seed Verification Code Sample (OTP)
        // -------------------------------------------------------------
        if (!await context.VerificationCodes.AnyAsync())
        {
            await context.VerificationCodes.AddAsync(new VerificationCode
            {
                Id = Guid.NewGuid(),
                UserId = userSingle1Id,
                Code = "123456",
                ExpirationTime = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false,
                Attempts = 0,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }
    }
}