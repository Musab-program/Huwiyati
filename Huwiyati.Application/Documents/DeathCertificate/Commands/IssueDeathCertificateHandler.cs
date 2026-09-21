namespace Huwiyati.Application.Documents.DeathCertificate.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.DeathCertificate.DTOs;

public class IssueDeathCertificateHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;

    public IssueDeathCertificateHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
    }

    public async Task<ApiResponse<DeathCertificateDto>> IssueAsync(
        IssueDeathCertificateCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify Hospital Branch exists and belongs to a valid Hospital organization
        var hospitalBranch = await _context.OrganizationBranches
            .AsNoTracking()
            .Where(b => b.Id == command.HospitalBranchId && b.IsActive)
            .Select(b => new { b.Id, b.BranchName, OrgName = b.Organization.Name })
            .FirstOrDefaultAsync(cancellationToken);

        if (hospitalBranch == null)
        {
            return ApiResponse<DeathCertificateDto>.Failure(
                "Specified hospital branch was not found or is inactive.", statusCode: 404);
        }

        // 2. Validate Civil Registry Issuing Branch via central extension method
        var branchResult = await _context.ValidateCivilRegistryBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branchResult.IsValid)
        {
            return ApiResponse<DeathCertificateDto>.Failure(branchResult.ErrorMessage, statusCode: branchResult.StatusCode);
        }

        // 3. Retrieve Person by National Number and verify existence and status
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<DeathCertificateDto>.Failure("Deceased person record with the provided national number was not found.", statusCode: 404);
        }

        if (person.PersonStatus == PersonStatus.Deceased)
        {
            return ApiResponse<DeathCertificateDto>.Failure("This person is already registered as deceased in the system.", statusCode: 400);
        }

        // 4. Check if Death Certificate already exists for this person
        var hasExistingDeathCert = await _context.DeathCertificates
            .AsNoTracking()
            .AnyAsync(d => d.PersonId == person.Id, cancellationToken);

        if (hasExistingDeathCert)
        {
            return ApiResponse<DeathCertificateDto>.Failure("A death certificate has already been issued for this person.", statusCode: 400);
        }

        // 5. Generate Official Death Certificate Number ("04...") using command IssuingBranchId
        var certificateNumber = await _documentNumberGenerator.GenerateDeathCertificateNumberAsync(command.IssuingBranchId, cancellationToken);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // 6. Create Death Certificate entity
        var deathCertificate = new DeathCertificate
        {
            PersonId = person.Id,
            HospitalBranchId = command.HospitalBranchId,
            IssuingBranchId = command.IssuingBranchId,
            CertificateNumber = certificateNumber,
            DeathDate = command.DeathDate,
            PlaceOfDeath = command.PlaceOfDeath,
            CauseOfDeath = command.CauseOfDeath,
            IssueDate = today
        };

        await _context.DeathCertificates.AddAsync(deathCertificate, cancellationToken);

        // 7. Update Person status to Deceased
        person.PersonStatus = PersonStatus.Deceased;

        // 8. Update active FamilyMember records status to Deceased and set LeftAt date
        var activeFamilyMembers = await _context.FamilyMembers
            .Where(m => m.PersonId == person.Id && m.Status == FamilyMemberStatus.Active)
            .ToListAsync(cancellationToken);

        foreach (var member in activeFamilyMembers)
        {
            member.Status = FamilyMemberStatus.Deceased;
            member.LeftAt = command.DeathDate.ToDateTime(TimeOnly.MinValue);
        }

        // 9. Save all changes in a single atomic database transaction
        await _context.SaveChangesAsync(cancellationToken);

        // 10. Construct response DTO
        var responseDto = new DeathCertificateDto
        {
            Id = deathCertificate.Id,
            CertificateNumber = deathCertificate.CertificateNumber,
            IssueDate = deathCertificate.IssueDate,

            PersonId = person.Id,
            NationalNumber = person.NationalNumber,
            FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender.ToString(),

            DeathDate = deathCertificate.DeathDate,
            PlaceOfDeath = deathCertificate.PlaceOfDeath,
            CauseOfDeath = deathCertificate.CauseOfDeath,

            HospitalBranchId = hospitalBranch.Id,
            HospitalName = hospitalBranch.BranchName,

            IssuingBranchId = branchResult.BranchId,
            IssuingBranchName = branchResult.BranchName,

            CreatedAt = deathCertificate.CreatedAt
        };

        return ApiResponse<DeathCertificateDto>.Success(
            responseDto, message: "Death certificate issued successfully and person status updated to Deceased.", statusCode: 201);
    }
}
