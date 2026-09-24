namespace Huwiyati.Application.Documents.Passport.Commands;

using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Documents.Passport.DTOs;
using Huwiyati.Domain.Entities.Documents;
using Huwiyati.Domain.Enums;

// Handler carrying out the business logic for adding a travel record
public class AddTravelRecordHandler
{
    private readonly IApplicationDbContext _context;

    public AddTravelRecordHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<TravelRecordDto>> AddAsync(
        AddTravelRecordCommand command,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate Immigration & Border Branch
        var branchResult = await _context.ValidateImmigrationBranchAsync(command.IssuingBranchId, cancellationToken);
        if (!branchResult.IsValid)
        {
            return ApiResponse<TravelRecordDto>.Failure(branchResult.ErrorMessage, statusCode: branchResult.StatusCode);
        }

        // 2. Fetch Passport along with Person details by Passport Number
        var passport = await _context.Passports
            .Include(p => p.Person)
            .FirstOrDefaultAsync(p => p.PassportNumber == command.PassportNumber.Trim(), cancellationToken);

        if (passport == null)
        {
            return ApiResponse<TravelRecordDto>.Failure(
                $"Passport with number '{command.PassportNumber}' was not found.", statusCode: 404);
        }

        // 3. Ensure Passport is Active
        if (passport.Status != PassportStatus.Active)
        {
            return ApiResponse<TravelRecordDto>.Failure(
                "Travel record can only be added to an active Passport.", statusCode: 400);
        }

        // 4. Create TravelRecord entity
        var travelRecord = new TravelRecord
        {
            PassportId = passport.Id,
            IssuingBranchId = command.IssuingBranchId,
            Country = command.Country.Trim(),
            EntryDate = command.EntryDate,
            ExitDate = command.ExitDate,
            CreatedAt = DateTime.UtcNow
        };

        await _context.TravelRecords.AddAsync(travelRecord, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 5. Map response DTO
        var responseDto = new TravelRecordDto
        {
            Id = travelRecord.Id,
            PassportId = passport.Id,
            PassportNumber = passport.PassportNumber,
            PersonId = passport.PersonId,
            PersonFullName = $"{passport.Person.FirstName} {passport.Person.FatherName} {passport.Person.GrandfatherName} {passport.Person.FamilyName}".Trim(),
            NationalNumber = passport.Person.NationalNumber,
            IssuingBranchId = branchResult.BranchId,
            IssuingBranchName = branchResult.BranchName,
            Country = travelRecord.Country,
            EntryDate = travelRecord.EntryDate,
            ExitDate = travelRecord.ExitDate,
            CreatedAt = travelRecord.CreatedAt
        };

        return ApiResponse<TravelRecordDto>.Success(
            responseDto, message: "Travel record added successfully.", statusCode: 201);
    }
}
