namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Handler implementing the business logic for updating Person details linked to an active National ID Card
public class UpdatePersonDataHandler
{
    private readonly IApplicationDbContext _context;

    public UpdatePersonDataHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<NationalIdCardDto>> UpdateAsync(
        UpdatePersonDataCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Find Person by National Number
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "No citizen record found with the provided National Number.", statusCode: 404);
        }

        // 2. Project active National ID Card details using Select instead of Include for optimal performance
        var activeCardDto = await _context.NationalIdCards
            .Where(c => c.PersonId == person.Id && c.Status == NationalIdCardStatus.Active)
            .Select(c => new NationalIdCardDto
            {
                Id = c.Id,
                PersonId = c.PersonId,
                NationalNumber = person.NationalNumber,
                FullName = string.Empty, // Will be populated after updating Person properties
                IssuingBranchId = c.IssuingBranchId,
                BranchName = c.OrganizationBranch.BranchName,
                IssueDate = c.IssueDate,
                ExpiryDate = c.ExpiryDate,
                QrCodePayload = c.QrCodePayload,
                Status = c.Status.ToString(),
                CreatedAt = c.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (activeCardDto == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "No active National ID Card found for this citizen.", statusCode: 404);
        }

        // 3. Update Person properties from command
        person.FirstName = command.FirstName;
        person.FatherName = command.FatherName;
        person.GrandfatherName = command.GrandfatherName;
        person.FamilyName = command.FamilyName;
        person.DateOfBirth = command.DateOfBirth;
        person.PlaceOfBirth = command.PlaceOfBirth;
        person.MaritalStatus = command.MaritalStatus;
        person.Governorate = command.Governorate;
        person.District = command.District;
        person.AddressDetails = command.AddressDetails;
        person.PhotoUrl = command.PhotoUrl;
        person.BloodGroup = command.BloodGroup;
        person.LastModifiedAt = DateTime.UtcNow;

        _context.Persons.Update(person);
        await _context.SaveChangesAsync(cancellationToken);

        // 4. Update projected FullName and return response DTO
        activeCardDto.FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim();

        return ApiResponse<NationalIdCardDto>.Success(
            activeCardDto, message: "National ID Card person data updated successfully.", statusCode: 200);
    }
}
