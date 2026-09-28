namespace Huwiyati.Application.Requests.Commands;

using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Extensions;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.DTOs;
using Huwiyati.Domain.Entities.CivilRegistry;
using Huwiyati.Domain.Entities.Requests;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;

// Handler carrying out business logic for initiating a new ServiceRequest
public class CreateServiceRequestHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IRequestNumberGenerator _requestNumberGenerator;
    private readonly IServicePayloadValidator _payloadValidator;

    public CreateServiceRequestHandler(
        IApplicationDbContext context,
        IRequestNumberGenerator requestNumberGenerator,
        IServicePayloadValidator payloadValidator)
    {
        _context = context;
        _requestNumberGenerator = requestNumberGenerator;
        _payloadValidator = payloadValidator;
    }

    public async Task<ApiResponse<ServiceRequestDto>> CreateAsync(
        CreateServiceRequestCommand command,
        Guid? currentPersonId = null,
        CancellationToken cancellationToken = default)
    {
        // 1. Validate processing Branch existence and active status via Extension Method
        var branchResult = await _context.ValidateActiveBranchAsync(command.BranchId, cancellationToken);
        if (!branchResult.IsValid)
        {
            return ApiResponse<ServiceRequestDto>.Failure(branchResult.ErrorMessage, statusCode: branchResult.StatusCode);
        }

        // 2. Resolve target Person entity (From Command PersonId, NationalNumber, or Current User Claim)
        Person? person = null;

        if (command.PersonId.HasValue && command.PersonId.Value != Guid.Empty)
        {
            person = await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == command.PersonId.Value, cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(command.NationalNumber))
        {
            person = await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.NationalNumber == command.NationalNumber, cancellationToken);
        }
        else if (currentPersonId.HasValue && currentPersonId.Value != Guid.Empty)
        {
            person = await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == currentPersonId.Value, cancellationToken);
        }

        if (person == null)
        {
            return ApiResponse<ServiceRequestDto>.Failure(
                "Target Person record associated with the request was not found.", statusCode: 404);
        }

        // 3. Validate ServiceType existence and active state
        var serviceType = await _context.ServiceTypes
            .FirstOrDefaultAsync(st => st.Id == command.ServiceTypeId && st.IsActive, cancellationToken);

        if (serviceType == null)
        {
            return ApiResponse<ServiceRequestDto>.Failure(
                "Specified service type does not exist or is currently inactive.", statusCode: 400);
        }

        // 4. Validate payload format against specific ServiceType schema rules
        var payloadValidation = _payloadValidator.ValidatePayload(serviceType.Code, command.RequestDataJson);
        if (!payloadValidation.IsValid)
        {
            return ApiResponse<ServiceRequestDto>.Failure(
                payloadValidation.ErrorMessage ?? "Invalid service payload.", statusCode: payloadValidation.StatusCode);
        }

        // 5. Generate unique Request Number
        var requestNumber = await _requestNumberGenerator.GenerateRequestNumberAsync(cancellationToken);

        // 6. Instantiate ServiceRequest entity
        var request = new ServiceRequest
        {
            RequestNumber = requestNumber,
            PersonId = person.Id,
            ServiceTypeId = serviceType.Id,
            BranchId = branchResult.BranchId,
            Status = RequestStatus.Pending,
            RequestDataJson = payloadValidation.SerializedJson,
            SubmissionDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        await _context.ServiceRequests.AddAsync(request, cancellationToken);

        // 7. Explicitly insert initial RequestStatusHistory record
        var initialHistory = new RequestStatusHistory
        {
            ServiceRequestId = request.Id,
            Status = RequestStatus.Pending,
            Note = "Request submitted successfully by applicant.",
            CreatedAt = DateTime.UtcNow
        };

        await _context.RequestStatusHistories.AddAsync(initialHistory, cancellationToken);

        // 8. Persist transaction to SQL Server
        await _context.SaveChangesAsync(cancellationToken);

        // 9. Map response DTO
        var responseDto = new ServiceRequestDto
        {
            Id = request.Id,
            RequestNumber = request.RequestNumber,
            PersonId = person.Id,
            PersonFullName = $"{person.FirstName} {person.FatherName} {person.GrandfatherName} {person.FamilyName}".Trim(),
            NationalNumber = person.NationalNumber,
            ServiceTypeId = serviceType.Id,
            ServiceTypeName = serviceType.Name,
            ServiceTypeCode = serviceType.Code,
            BranchId = branchResult.BranchId,
            BranchName = branchResult.BranchName,
            Status = request.Status,
            RequestDataJson = request.RequestDataJson,
            SubmissionDate = request.SubmissionDate,
            CreatedAt = request.CreatedAt,
            CreatedBy = request.CreatedBy,
            StatusHistory = new List<RequestStatusHistoryDto>
            {
                new RequestStatusHistoryDto
                {
                    Id = initialHistory.Id,
                    ServiceRequestId = request.Id,
                    RequestNumber = request.RequestNumber,
                    Status = initialHistory.Status,
                    Note = initialHistory.Note,
                    CreatedAt = initialHistory.CreatedAt,
                    CreatedBy = initialHistory.CreatedBy
                }
            }
        };

        return ApiResponse<ServiceRequestDto>.Success(
            responseDto, message: "Service request created successfully.", statusCode: 201);
    }
}
