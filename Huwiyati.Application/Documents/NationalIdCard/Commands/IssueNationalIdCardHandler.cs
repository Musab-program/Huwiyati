namespace Huwiyati.Application.Documents.NationalIdCard.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;
using Huwiyati.Application.Documents.NationalIdCard.DTOs;

// Handler implementing the business logic for issuing a National ID Card
public class IssueNationalIdCardHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IDocumentNumberGenerator _documentNumberGenerator;

    public IssueNationalIdCardHandler(
        IApplicationDbContext context,
        IDocumentNumberGenerator documentNumberGenerator)
    {
        _context = context;
        _documentNumberGenerator = documentNumberGenerator;
    }

    public async Task<ApiResponse<NationalIdCardDto>> IssueAsync(
        IssueNationalIdCardCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Verify issuing branch exists
            var branch = await _context.OrganizationBranches
        .AsNoTracking()
        .Where(b => b.Id == command.IssuingBranchId)
        .Select(b => new
        {
            b.Id,
            b.BranchName,
            b.IsActive,
            OrganizationIsActive = b.Organization.IsActive,
            OrganizationName = b.Organization.Name
        })
        .FirstOrDefaultAsync(cancellationToken);

            if (branch == null)
            {
                return ApiResponse<NationalIdCardDto>.Failure(
                    "Specified issuing branch does not exist.", statusCode: 404);
            }

            if (!branch.IsActive || !branch.OrganizationIsActive)
            {
                return ApiResponse<NationalIdCardDto>.Failure(
                    "Specified issuing branch or its parent organization is inactive.", statusCode: 400);
            }

            if (!branch.OrganizationName.Contains("الأحوال المدنية"))
            {
                return ApiResponse<NationalIdCardDto>.Failure(
                    "Document cards can only be processed by Civil Registry branches (الأحوال المدنية).", statusCode: 400);
            }

        Person person;

        // 2. Case A: Person exists by PersonId
        if (command.PersonId.HasValue)
        {
            var existingPerson = await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == command.PersonId.Value, cancellationToken);

            if (existingPerson == null)
            {
                return ApiResponse<NationalIdCardDto>.Failure(
                    "Person with the provided ID was not found.", statusCode: 404);
            }

            person = existingPerson;
        }
        else // Case B: Create new Person and Generate 11-digit National Number
        {
            var generatedNationalNumber = await _documentNumberGenerator.GenerateNationalNumberAsync(command.IssuingBranchId, cancellationToken);

            person = new Person
            {
                NationalNumber = generatedNationalNumber,
                FirstName = command.FirstName!,
                FatherName = command.FatherName!,
                GrandfatherName = command.GrandfatherName!,
                FamilyName = command.FamilyName!,
                DateOfBirth = command.DateOfBirth!.Value,
                PlaceOfBirth = command.PlaceOfBirth!,
                Gender = command.Gender!.Value,
                Nationality = command.Nationality ?? "Yemeni",
                MaritalStatus = command.MaritalStatus ?? MaritalStatus.Single,
                Governorate = command.Governorate!,
                District = command.District!,
                AddressDetails = command.AddressDetails!,
                PhotoUrl = command.PhotoUrl,
                BloodGroup = command.BloodGroup!.Value,
                PersonStatus = PersonStatus.Active
            };

            await _context.Persons.AddAsync(person, cancellationToken);
        }

        // 3. Ensure person does not already have an Active National ID Card
        var activeCardExists = await _context.NationalIdCards
            .AnyAsync(c => c.PersonId == person.Id && c.Status == NationalIdCardStatus.Active, cancellationToken);

        if (activeCardExists)
        {
            return ApiResponse<NationalIdCardDto>.Failure(
                "This person already possesses an active National ID Card.", statusCode: 400);
        }

        // 4. Calculate IssueDate (default Today) and ExpiryDate (fixed +10 years automatically)
        var issueDate = command.IssueDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
        var expiryDate = issueDate.AddYears(10);

        // 5. Create NationalIdCard entity (Id generated via BaseEntity Guid.CreateVersion7())
        var card = new NationalIdCard
        {
            PersonId = person.Id,
            IssuingBranchId = command.IssuingBranchId,
            IssueDate = issueDate,
            ExpiryDate = expiryDate,
            QrCodePayload = $"NAT-{person.NationalNumber}",
            Status = NationalIdCardStatus.Active,
            CreatedAt = DateTime.UtcNow,
        };

        await _context.NationalIdCards.AddAsync(card, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 6. Map to DTO and return response
        var responseDto = new NationalIdCardDto
        {
            Id = card.Id,
            PersonId = person.Id,
            NationalNumber = person.NationalNumber,
            FullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            IssuingBranchId = branch.Id,
            BranchName = branch.BranchName,
            IssueDate = card.IssueDate,
            ExpiryDate = card.ExpiryDate,
            QrCodePayload = card.QrCodePayload,
            Status = card.Status.ToString(),
            CreatedAt = card.CreatedAt
        };

        return ApiResponse<NationalIdCardDto>.Success(
            responseDto, message: "National ID Card issued successfully.", statusCode: 201);
    }
}