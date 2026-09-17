namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Handler implementing the business logic for renewing a National ID Card according to 3-month eligibility rules
public class RenewNationalIdCardHandler
{
    private readonly IApplicationDbContext _context;

    public RenewNationalIdCardHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<NationalIdCardDto>> RenewAsync(
        RenewNationalIdCardCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Search for Person by National Number
        var person = await _context.Persons
            .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);

        if (person == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "No citizen record found with the provided National Number.", statusCode: 404);
        }

        // 2. Verify issuing branch exists
        var branch = await _context.OrganizationBranches
            .FirstOrDefaultAsync(b => b.Id == command.IssuingBranchId, cancellationToken);

        if (branch == null)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "Specified issuing branch does not exist.", statusCode: 404);
        }

        // 3. Find existing Active National ID Card for this Person
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var activeCard = await _context.NationalIdCards
            .FirstOrDefaultAsync(c => c.PersonId == person.Id && c.Status == NationalIdCardStatus.Active, cancellationToken);

        // If an Active card exists (meaning it hasn't turned Expired automatically yet), check remaining days
        if (activeCard != null)
        {
            var daysRemaining = activeCard.ExpiryDate.DayNumber - today.DayNumber;

            // If remaining valid days is strictly more than 3 months (90 days), disallow early renewal
            if (daysRemaining > 90)
            {
                return ApiResponse<NationalIdCardDto>.Failure(
                    $"Card cannot be renewed yet. Renewal is only allowed within 3 months (90 days) prior to expiry date. Remaining valid days: {daysRemaining}.",
                    statusCode: 400);
            }

            // Remaining days <= 90 -> Mark the active card as Expired
            activeCard.Status = NationalIdCardStatus.Expired;
        }

        // 4. Update Person optional profile fields cleanly
        UpdatePersonProfileIfProvided(person, command);

        // 5. Calculate new IssueDate (today UTC) and ExpiryDate (+10 years)
        var newIssueDate = today;
        var newExpiryDate = newIssueDate.AddYears(10);

        // 6. Create NEW NationalIdCard entity with a new Guid Version 7
        var newCard = new NationalIdCard
        {
            PersonId = person.Id,
            IssuingBranchId = command.IssuingBranchId,
            IssueDate = newIssueDate,
            ExpiryDate = newExpiryDate,
            QrCodePayload = $"NAT-{person.NationalNumber}",
            Status = NationalIdCardStatus.Active
        };

        await _context.NationalIdCards.AddAsync(newCard, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 7. Map to DTO and return response
        var responseDto = new NationalIdCardDto
        {
            Id = newCard.Id,
            PersonId = person.Id,
            NationalNumber = person.NationalNumber,
            FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            IssuingBranchId = branch.Id,
            BranchName = branch.BranchName,
            IssueDate = newCard.IssueDate,
            ExpiryDate = newCard.ExpiryDate,
            QrCodePayload = newCard.QrCodePayload,
            Status = newCard.Status.ToString(),
            CreatedAt = newCard.CreatedAt
        };

        return ApiResponse<NationalIdCardDto>.Success(
            responseDto, message: "National ID Card renewed successfully with a new 10-year validity.", statusCode: 200);
    }

    // Encapsulated method updating optional Person profile fields on the tracked reference object
    private static void UpdatePersonProfileIfProvided(Person person, RenewNationalIdCardCommand command)
    {
        if (command.MaritalStatus.HasValue) person.MaritalStatus = command.MaritalStatus.Value;
        if (!string.IsNullOrWhiteSpace(command.Governorate)) person.Governorate = command.Governorate;
        if (!string.IsNullOrWhiteSpace(command.District)) person.District = command.District;
        if (!string.IsNullOrWhiteSpace(command.AddressDetails)) person.AddressDetails = command.AddressDetails;
        if (!string.IsNullOrWhiteSpace(command.PhotoUrl)) person.PhotoUrl = command.PhotoUrl;
    }
}
