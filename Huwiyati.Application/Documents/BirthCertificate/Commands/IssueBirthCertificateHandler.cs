namespace Huwiyati.Application.Documents.BirthCertificate.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Entities.Family;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.BirthCertificate.DTOs;

public class IssueBirthCertificateHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;

    public IssueBirthCertificateHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
    }

    public async Task<ApiResponse<BirthCertificateDto>> IssueAsync(
        IssueBirthCertificateCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate Civil Registry Issuing Branch via central extension method
        var branchResult = await _context.ValidateCivilRegistryBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branchResult.IsValid)
        {
            return ApiResponse<BirthCertificateDto>.Failure(branchResult.ErrorMessage, statusCode: branchResult.StatusCode);
        }

        // 2. Verify Father existence by National Number
        var father = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.FatherNationalNumber, cancellationToken);

        if (father == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Father person record with the provided national number was not found.", statusCode: 404);
        }

        // 3. Verify Mother existence by National Number
        var mother = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.MotherNationalNumber, cancellationToken);

        if (mother == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Mother person record with the provided national number was not found.", statusCode: 404);
        }

        // 4. Verify Family and Hospital Branch existence, and ensure Father is Head of Family
        var family = await _context.Families
            .FirstOrDefaultAsync(f => f.Id == command.FamilyId, cancellationToken);

        if (family == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Target family record was not found.", statusCode: 404);
        }

        if (family.HeadOfFamilyPersonId != father.Id)
        {
            return ApiResponse<BirthCertificateDto>.Failure("The specified father is not the registered head of this family.", statusCode: 400);
        }

        var hospitalBranch = await _context.OrganizationBranches
            .FirstOrDefaultAsync(b => b.Id == command.HospitalBranchId, cancellationToken);

        if (hospitalBranch == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Hospital branch was not found.", statusCode: 404);
        }

        // 5. Generate National Number for child and Birth Certificate Number
        var childNationalNumber = await _documentNumberGenerator.GenerateNationalNumberAsync(command.IssuingBranchId, cancellationToken);
        var certificateNumber = await _documentNumberGenerator.GenerateBirthCertificateNumberAsync(command.IssuingBranchId, cancellationToken);

        // 6. Create Child Person entity
        var childPerson = new Person
        {
            NationalNumber = childNationalNumber,
            FirstName = command.FirstName,
            FatherName = father.FirstName,
            GrandfatherName = father.FatherName,
            FamilyName = father.FamilyName,
            DateOfBirth = command.DateOfBirth,
            PlaceOfBirth = command.PlaceOfBirth,
            Gender = command.Gender,
            Nationality = father.Nationality ?? "Yemeni",
            MaritalStatus = MaritalStatus.Single,
            Governorate = command.Governorate,
            District = command.District,
            AddressDetails = command.AddressDetails,
            BloodGroup = command.BloodGroup,
            PersonStatus = PersonStatus.Active
        };

        await _context.Persons.AddAsync(childPerson, cancellationToken);

        // 7. Create Birth Certificate entity
        var issueDate = command.IssueDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var birthCertificate = new BirthCertificate
        {
            ChildPersonId = childPerson.Id,
            FatherPersonId = father.Id,
            MotherPersonId = mother.Id,
            HospitalBranchId = command.HospitalBranchId,
            CertificateNumber = certificateNumber,
            IssueDate = issueDate
        };

        await _context.BirthCertificates.AddAsync(birthCertificate, cancellationToken);

        // 8. Create Family Member entity
        var relationshipType = command.Gender == Gender.Male 
            ? RelationshipType.Son 
            : RelationshipType.Daughter;

        var familyMember = new FamilyMember
        {
            FamilyId = command.FamilyId,
            PersonId = childPerson.Id,
            RelationshipType = relationshipType,
            Status = FamilyMemberStatus.Active,
            JoinedAt = DateTime.UtcNow
        };

        await _context.FamilyMembers.AddAsync(familyMember, cancellationToken);

        // 9. Save all changes in a single atomic database transaction
        await _context.SaveChangesAsync(cancellationToken);

        // 10. Construct response DTO
        var responseDto = new BirthCertificateDto
        {
            Id = birthCertificate.Id,
            CertificateNumber = birthCertificate.CertificateNumber,
            IssueDate = birthCertificate.IssueDate,

            ChildPersonId = childPerson.Id,
            ChildNationalNumber = childPerson.NationalNumber,
            ChildFullName = $"{childPerson.FirstName} {childPerson.FatherName} {childPerson.GrandfatherName} {childPerson.FamilyName}".Trim(),
            Gender = childPerson.Gender.ToString(),
            DateOfBirth = childPerson.DateOfBirth,
            PlaceOfBirth = childPerson.PlaceOfBirth,

            FatherPersonId = father.Id,
            FatherNationalNumber = father.NationalNumber,
            FatherFullName = $"{father.FirstName} {father.FatherName} {father.GrandfatherName} {father.FamilyName}".Trim(),

            MotherPersonId = mother.Id,
            MotherNationalNumber = mother.NationalNumber,
            MotherFullName = $"{mother.FirstName} {mother.FatherName} {mother.GrandfatherName} {mother.FamilyName}".Trim(),

            HospitalBranchId = hospitalBranch.Id,
            HospitalName = hospitalBranch.BranchName,
            FamilyId = command.FamilyId,
            CreatedAt = birthCertificate.CreatedAt
        };

        return ApiResponse<BirthCertificateDto>.Success(
            responseDto, message: "Birth certificate issued successfully.", statusCode: 201);
    }
}
