namespace Huwiyati.Application.Documents.BirthCertificate.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.BirthCertificate.DTOs;

public class UpdateChildDataHandler
{
    private readonly IApplicationDbContext _context;

    public UpdateChildDataHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<BirthCertificateDto>> UpdateAsync(
        UpdateChildDataCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Fetch BirthCertificate entity
        var birthCertificate = await _context.BirthCertificates
            .FirstOrDefaultAsync(b => b.Id == command.BirthCertificateId, cancellationToken);

        if (birthCertificate == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Specified birth certificate record was not found.", statusCode: 404);
        }

        // 2. Fetch linked Child Person entity
        var childPerson = await _context.Persons
            .FirstOrDefaultAsync(p => p.Id == birthCertificate.ChildPersonId, cancellationToken);

        if (childPerson == null)
        {
            return ApiResponse<BirthCertificateDto>.Failure("Child person record associated with this birth certificate was not found.", statusCode: 404);
        }

        // 3. Update Child Person properties
        childPerson.FirstName = command.FirstName;
        childPerson.DateOfBirth = command.DateOfBirth;
        childPerson.PlaceOfBirth = command.PlaceOfBirth;
        childPerson.Gender = command.Gender;
        childPerson.BloodGroup = command.BloodGroup;
        childPerson.Governorate = command.Governorate;
        childPerson.District = command.District;
        childPerson.AddressDetails = command.AddressDetails;

        // 4. Save Changes to database
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Query updated dto projection cleanly via Select
        var responseDto = await _context.BirthCertificates
            .AsNoTracking()
            .Where(b => b.Id == birthCertificate.Id)
            .Select(b => new BirthCertificateDto
            {
                Id = b.Id,
                CertificateNumber = b.CertificateNumber,
                IssueDate = b.IssueDate,

                ChildPersonId = b.ChildPersonId,
                ChildNationalNumber = b.ChildPerson.NationalNumber,
                ChildFullName = $"{b.ChildPerson.FirstName} {b.ChildPerson.FatherName} {b.ChildPerson.GrandfatherName} {b.ChildPerson.FamilyName}".Trim(),
                Gender = b.ChildPerson.Gender.ToString(),
                DateOfBirth = b.ChildPerson.DateOfBirth,
                PlaceOfBirth = b.ChildPerson.PlaceOfBirth,

                FatherPersonId = b.FatherPersonId,
                FatherNationalNumber = b.FatherPerson.NationalNumber,
                FatherFullName = $"{b.FatherPerson.FirstName} {b.FatherPerson.FatherName} {b.FatherPerson.GrandfatherName} {b.FatherPerson.FamilyName}".Trim(),

                MotherPersonId = b.MotherPersonId,
                MotherNationalNumber = b.MotherPerson.NationalNumber,
                MotherFullName = $"{b.MotherPerson.FirstName} {b.MotherPerson.FatherName} {b.MotherPerson.GrandfatherName} {b.MotherPerson.FamilyName}".Trim(),

                HospitalBranchId = b.HospitalBranchId,
                HospitalName = b.HospitalBranch.BranchName,
                CreatedAt = b.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        return ApiResponse<BirthCertificateDto>.Success(responseDto!, message: "Child personal data updated successfully.");
    }
}
