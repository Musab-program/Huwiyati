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
        // 2. Seed Organizations
        // -------------------------------------------------------------
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

        // Fetch actual Organization IDs from DB
        var civilRegistryOrg = await context.Organizations.FirstOrDefaultAsync(o => o.Name.Contains("الأحوال المدنية"));
        var hospitalsOrg = await context.Organizations.FirstOrDefaultAsync(o => o.Name.Contains("المستشفيات"));

        var civilRegistryOrgId = civilRegistryOrg?.Id ?? orgCivilRegistryId;
        var hospitalsOrgId = hospitalsOrg?.Id ?? orgHospitalsId;

        // -------------------------------------------------------------
        // 3. Seed Organization Branches
        // -------------------------------------------------------------
        var branchCivilRegistrySanaaId = Guid.Parse("018f7d9a-2000-7000-8000-000000000001");
        var branchCivilRegistryAdenId  = Guid.Parse("018f7d9a-2000-7000-8000-000000000002");
        var branchHospitalThawraId     = Guid.Parse("018f7d9a-2000-7000-8000-000000000003");
        var branchHospitalJumhuriId    = Guid.Parse("018f7d9a-2000-7000-8000-000000000004");

        var seedBranches = new List<OrganizationBranch>
        {
            new OrganizationBranch
            {
                Id = branchCivilRegistrySanaaId,
                OrganizationId = civilRegistryOrgId,
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
                OrganizationId = civilRegistryOrgId,
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
                OrganizationId = hospitalsOrgId,
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
                OrganizationId = hospitalsOrgId,
                BranchName = "مستشفى الجمهوري التعليمي",
                Governorate = "أمانة العاصمة",
                District = "التحرير",
                AddressDetails = "شارع الزبيري",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        foreach (var branch in seedBranches)
        {
            if (!await context.OrganizationBranches.AnyAsync(b => b.Id == branch.Id))
            {
                await context.OrganizationBranches.AddAsync(branch);
            }
        }
        await context.SaveChangesAsync();

        // Fetch actual Branch IDs from DB
        var sanaaBranch = await context.OrganizationBranches.FirstOrDefaultAsync(b => b.BranchName.Contains("صنعاء"))
            ?? await context.OrganizationBranches.FirstOrDefaultAsync(b => b.Id == branchCivilRegistrySanaaId);

        var adenBranch = await context.OrganizationBranches.FirstOrDefaultAsync(b => b.BranchName.Contains("عدن"))
            ?? await context.OrganizationBranches.FirstOrDefaultAsync(b => b.Id == branchCivilRegistryAdenId);

        var thawraHospitalBranch = await context.OrganizationBranches.FirstOrDefaultAsync(b => b.BranchName.Contains("الثورة"))
            ?? await context.OrganizationBranches.FirstOrDefaultAsync(b => b.Id == branchHospitalThawraId);

        var sanaaBranchId = sanaaBranch?.Id ?? branchCivilRegistrySanaaId;
        var adenBranchId = adenBranch?.Id ?? branchCivilRegistryAdenId;
        var thawraHospitalBranchId = thawraHospitalBranch?.Id ?? branchHospitalThawraId;

        // -------------------------------------------------------------
        // 4. Seed Persons & Retrieve Actual Database Person Entities
        // -------------------------------------------------------------
        var pSuperAdminId = Guid.Parse("018f7d9a-3000-7000-8000-000000000001");
        var pAdminId      = Guid.Parse("018f7d9a-3000-7000-8000-000000000002");
        var pEmployeeId   = Guid.Parse("018f7d9a-3000-7000-8000-000000000003");
        var pFatherId     = Guid.Parse("018f7d9a-3000-7000-8000-000000000004");
        var pMotherId     = Guid.Parse("018f7d9a-3000-7000-8000-000000000005");
        var pSonId        = Guid.Parse("018f7d9a-3000-7000-8000-000000000006");
        var pDauId        = Guid.Parse("018f7d9a-3000-7000-8000-000000000007");

        var seedPersonsList = new List<Person>
        {
            new Person
            {
                Id = pSuperAdminId,
                NationalNumber = "01011135650",
                FirstName = "مصعب",
                FatherName = "محمد",
                GrandfatherName = "أحمد",
                FamilyName = "النجري",
                DateOfBirth = new DateOnly(1995, 1, 1),
                PlaceOfBirth = "صنعاء",
                BloodGroup = BloodGroup.APositive,
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
                Id = pAdminId,
                NationalNumber = "01011135651",
                FirstName = "أحمد",
                FatherName = "محمود",
                GrandfatherName = "علي",
                FamilyName = "المدير",
                DateOfBirth = new DateOnly(1990, 4, 10),
                PlaceOfBirth = "صنعاء",
                BloodGroup = BloodGroup.BPositive,
                Gender = Gender.Male,
                Nationality = "يمني",
                MaritalStatus = MaritalStatus.Married,
                Governorate = "أمانة العاصمة",
                District = "السبعين",
                AddressDetails = "شارع حدة",
                PersonStatus = PersonStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
            new Person
            {
                Id = pEmployeeId,
                NationalNumber = "01011131317",
                FirstName = "سارة",
                FatherName = "عبد المجيد",
                GrandfatherName = "محمد",
                FamilyName = "السالمي",
                DateOfBirth = new DateOnly(1998, 5, 20),
                PlaceOfBirth = "صنعاء",
                BloodGroup = BloodGroup.OPositive,
                Gender = Gender.Female,
                Nationality = "يمنية",
                MaritalStatus = MaritalStatus.Single,
                Governorate = "أمانة العاصمة",
                District = "التحرير",
                AddressDetails = "شارع التحرير",
                PersonStatus = PersonStatus.Active,
                CreatedAt = DateTime.UtcNow
            },
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
            }
        };

        foreach (var p in seedPersonsList)
        {
            if (!await context.Persons.AnyAsync(existing => existing.NationalNumber == p.NationalNumber || existing.Id == p.Id))
            {
                await context.Persons.AddAsync(p);
            }
        }
        await context.SaveChangesAsync();

        // Dynamically resolve actual Person entities from DB to guarantee valid FKs
        var superAdminPerson = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01011135650") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pSuperAdminId);
        var adminPerson      = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01011135651") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pAdminId);
        var employeePerson   = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01011131317") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pEmployeeId);
        var fatherPerson     = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01001000001") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pFatherId);
        var motherPerson     = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01001000002") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pMotherId);
        var sonPerson        = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01001000003") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pSonId);
        var daughterPerson   = await context.Persons.FirstOrDefaultAsync(p => p.NationalNumber == "01001000004") ?? await context.Persons.FirstOrDefaultAsync(p => p.Id == pDauId);

        // -------------------------------------------------------------
        // 5. Seed ApplicationUsers
        // -------------------------------------------------------------
        var userSuperAdminId = Guid.Parse("018f7d9a-4000-7000-8000-000000000001");
        var userAdminId      = Guid.Parse("018f7d9a-4000-7000-8000-000000000002");
        var userEmployeeId   = Guid.Parse("018f7d9a-4000-7000-8000-000000000003");
        var userCitizenId    = Guid.Parse("018f7d9a-4000-7000-8000-000000000004");

        var seedUsers = new[]
        {
            new { DefaultUserId = userSuperAdminId, Person = superAdminPerson, Email = "superadmin@huwiyati.com", PhoneNumber = "770000001", Role = AppRoles.SuperAdmin },
            new { DefaultUserId = userAdminId,      Person = adminPerson,      Email = "admin@huwiyati.com",      PhoneNumber = "770000002", Role = AppRoles.Admin },
            new { DefaultUserId = userEmployeeId,   Person = employeePerson,   Email = "employee@huwiyati.com",   PhoneNumber = "770000003", Role = AppRoles.Employee },
            new { DefaultUserId = userCitizenId,    Person = fatherPerson,     Email = "citizen@huwiyati.com",    PhoneNumber = "770000004", Role = AppRoles.Citizen }
        };

        foreach (var uData in seedUsers)
        {
            if (uData.Person == null) continue;

            var userByName = await userManager.FindByNameAsync(uData.Person.NationalNumber);
            var userByPerson = await context.Users.FirstOrDefaultAsync(u => u.PersonId == uData.Person.Id);

            ApplicationUser? targetUser = userByName ?? userByPerson;

            if (targetUser == null)
            {
                targetUser = new ApplicationUser
                {
                    Id = uData.DefaultUserId,
                    PersonId = uData.Person.Id,
                    UserName = uData.Person.NationalNumber,
                    Email = uData.Email,
                    PhoneNumber = uData.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true,
                    Status = AccountStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    ActivatedAt = DateTime.UtcNow
                };

                var createResult = await userManager.CreateAsync(targetUser, "Password123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(targetUser, uData.Role);
                }
            }

            if (targetUser != null && !await context.UserDevices.AnyAsync(d => d.UserId == targetUser.Id))
            {
                await context.UserDevices.AddAsync(new UserDevice
                {
                    Id = Guid.NewGuid(),
                    UserId = targetUser.Id,
                    DeviceName = "Mobile Device",
                    DeviceIdentifier = $"device-{uData.Person.NationalNumber}",
                    OperatingSystem = "Android",
                    IsTrusted = true,
                    LastLogin = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await context.SaveChangesAsync();

        // -------------------------------------------------------------
        // 6. Seed Employee Records (Guaranteed Valid User and Branch FKs)
        // -------------------------------------------------------------
        if (adminPerson != null)
        {
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.PersonId == adminPerson.Id);
            if (adminUser != null && !await context.Employees.AnyAsync(e => e.UserId == adminUser.Id))
            {
                await context.Employees.AddAsync(new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = adminUser.Id,
                    BranchId = adenBranchId,
                    EmployeeNumber = "EMP-001",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        if (employeePerson != null)
        {
            var staffUser = await context.Users.FirstOrDefaultAsync(u => u.PersonId == employeePerson.Id);
            if (staffUser != null && !await context.Employees.AnyAsync(e => e.UserId == staffUser.Id))
            {
                await context.Employees.AddAsync(new Employee
                {
                    Id = Guid.NewGuid(),
                    UserId = staffUser.Id,
                    BranchId = sanaaBranchId,
                    EmployeeNumber = "EMP-002",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await context.SaveChangesAsync();

        // -------------------------------------------------------------
        // 7. Seed Marriage Contract (Guaranteed Valid Husband & Wife FKs)
        // -------------------------------------------------------------
        MarriageContract? marriageContract = null;
        if (fatherPerson != null && motherPerson != null)
        {
            marriageContract = await context.MarriageContracts
                .FirstOrDefaultAsync(m => m.HusbandPersonId == fatherPerson.Id && m.WifePersonId == motherPerson.Id);

            if (marriageContract == null)
            {
                marriageContract = new MarriageContract
                {
                    Id = Guid.NewGuid(),
                    ContractNumber = "MAR-2015-0001",
                    HusbandPersonId = fatherPerson.Id,
                    WifePersonId = motherPerson.Id,
                    MarriageDate = new DateOnly(2015, 6, 20),
                    Status = MarriageStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                await context.MarriageContracts.AddAsync(marriageContract);
                await context.SaveChangesAsync();
            }
        }

        // -------------------------------------------------------------
        // 8. Seed Family & Family Members (Guaranteed Valid Person & Branch FKs)
        // -------------------------------------------------------------
        if (fatherPerson != null)
        {
            var family = await context.Families.FirstOrDefaultAsync(f => f.HeadOfFamilyPersonId == fatherPerson.Id || f.FamilyNumber == "02001000001");
            if (family == null)
            {
                family = new Family
                {
                    Id = Guid.NewGuid(),
                    FamilyNumber = "02001000001",
                    HeadOfFamilyPersonId = fatherPerson.Id,
                    IssuingBranchId = sanaaBranchId,
                    IssueDate = new DateOnly(2015, 7, 1),
                    ExpiryDate = new DateOnly(2025, 7, 1),
                    QrCodePayload = "FAM-02001000001",
                    Status = FamilyStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                await context.Families.AddAsync(family);
                await context.SaveChangesAsync();

                var familyMembers = new List<FamilyMember>();

                if (fatherPerson != null)
                    familyMembers.Add(new FamilyMember { Id = Guid.NewGuid(), FamilyId = family.Id, PersonId = fatherPerson.Id, RelationshipType = RelationshipType.Head, Status = FamilyMemberStatus.Active, JoinedAt = DateTime.UtcNow });

                if (motherPerson != null)
                    familyMembers.Add(new FamilyMember { Id = Guid.NewGuid(), FamilyId = family.Id, PersonId = motherPerson.Id, MarriageContractId = marriageContract?.Id, RelationshipType = RelationshipType.Wife, Status = FamilyMemberStatus.Active, JoinedAt = DateTime.UtcNow });

                if (sonPerson != null)
                    familyMembers.Add(new FamilyMember { Id = Guid.NewGuid(), FamilyId = family.Id, PersonId = sonPerson.Id, RelationshipType = RelationshipType.Son, Status = FamilyMemberStatus.Active, JoinedAt = DateTime.UtcNow });

                if (daughterPerson != null)
                    familyMembers.Add(new FamilyMember { Id = Guid.NewGuid(), FamilyId = family.Id, PersonId = daughterPerson.Id, RelationshipType = RelationshipType.Daughter, Status = FamilyMemberStatus.Active, JoinedAt = DateTime.UtcNow });

                if (familyMembers.Any())
                {
                    await context.FamilyMembers.AddRangeAsync(familyMembers);
                    await context.SaveChangesAsync();
                }
            }
        }

        // -------------------------------------------------------------
        // 9. Seed National ID Cards (Guaranteed Valid Person & Branch FKs)
        // -------------------------------------------------------------
        var eligiblePersonsForIdCard = new[]
        {
            new { Person = superAdminPerson, QrCode = "NAT-01011135650" },
            new { Person = fatherPerson,     QrCode = "NAT-01001000001" },
            new { Person = motherPerson,     QrCode = "NAT-01001000002" }
        };

        foreach (var item in eligiblePersonsForIdCard)
        {
            if (item.Person != null && !await context.NationalIdCards.AnyAsync(card => card.PersonId == item.Person.Id))
            {
                await context.NationalIdCards.AddAsync(new NationalIdCard
                {
                    Id = Guid.NewGuid(),
                    PersonId = item.Person.Id,
                    IssuingBranchId = sanaaBranchId,
                    IssueDate = new DateOnly(2020, 1, 1),
                    ExpiryDate = new DateOnly(2030, 1, 1),
                    QrCodePayload = item.QrCode,
                    Status = NationalIdCardStatus.Active,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        await context.SaveChangesAsync();

        // -------------------------------------------------------------
        // 10. Seed Birth Certificates (Guaranteed Valid Child, Father, Mother & Hospital FKs)
        // -------------------------------------------------------------
        var createdByUserId = (employeePerson != null ? (await context.Users.FirstOrDefaultAsync(u => u.PersonId == employeePerson.Id))?.Id.ToString() : null) ?? Guid.NewGuid().ToString();

        var seedCertificates = new[]
        {
            new { Child = sonPerson,      CertNo = "03001000001", IssueDate = new DateOnly(2020, 3, 12) },
            new { Child = daughterPerson, CertNo = "03001000002", IssueDate = new DateOnly(2024, 1, 3) }
        };

        foreach (var cert in seedCertificates)
        {
            if (cert.Child != null && fatherPerson != null && motherPerson != null)
            {
                if (!await context.BirthCertificates.AnyAsync(b => b.ChildPersonId == cert.Child.Id || b.CertificateNumber == cert.CertNo))
                {
                    await context.BirthCertificates.AddAsync(new BirthCertificate
                    {
                        Id = Guid.NewGuid(),
                        ChildPersonId = cert.Child.Id,
                        FatherPersonId = fatherPerson.Id,
                        MotherPersonId = motherPerson.Id,
                        HospitalBranchId = thawraHospitalBranchId,
                        CertificateNumber = cert.CertNo,
                        IssueDate = cert.IssueDate,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = createdByUserId
                    });
                }
            }
        }
        await context.SaveChangesAsync();
    }
}