using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.DTOs;
using Huwiyati.Domain.Entities.Requests;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Requests.Commands;

// Handler allowing applicants or submitter employees to cancel a pending ServiceRequest
public class CancelServiceRequestHandler
{
    private readonly IApplicationDbContext _context;

    public CancelServiceRequestHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ServiceRequestDto>> CancelAsync(
        CancelServiceRequestCommand command,
        Guid? currentPersonId = null,
        Guid? currentEmployeeUserId = null,
        CancellationToken cancellationToken = default)
    {
        // Fetch Request using Select (NO Include)
        var requestData = await _context.ServiceRequests
            .AsNoTracking()
            .Where(r => r.Id == command.ServiceRequestId)
            .Select(r => new
            {
                r.Id,
                r.RequestNumber,
                r.PersonId,
                r.ServiceTypeId,
                r.BranchId,
                r.Status,
                r.SubmissionDate,
                r.CreatedAt,
                r.CreatedBy,
                r.RequestDataJson,
                PersonFullName = $"{r.Person.FirstName} {r.Person.FatherName} {r.Person.GrandfatherName} {r.Person.FamilyName}".Trim(),
                NationalNumber = r.Person.NationalNumber,
                ServiceTypeName = r.ServiceType.Name,
                ServiceTypeCode = r.ServiceType.Code,
                BranchName = r.Branch.BranchName
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (requestData == null)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Service request not found.", statusCode: 404);
        }

        // Only Pending requests can be cancelled
        if (requestData.Status != RequestStatus.Pending)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Only pending requests can be cancelled.", statusCode: 400);
        }

        // Ownership / Authorization Check:
        // Case A: Citizen submitter check
        if (currentPersonId.HasValue && requestData.PersonId != currentPersonId.Value)
        {
            return ApiResponse<ServiceRequestDto>.Failure("You do not have permission to cancel this request.", statusCode: 403);
        }

        // Case B: Employee branch check (employee can only cancel pending requests assigned to their branch)
        if (currentEmployeeUserId.HasValue)
        {
            var employeeBranchId = await _context.Employees
                .AsNoTracking()
                .Where(e => e.UserId == currentEmployeeUserId.Value && e.IsActive)
                .Select(e => (Guid?)e.BranchId)
                .FirstOrDefaultAsync(cancellationToken);

            if (!employeeBranchId.HasValue || employeeBranchId.Value != requestData.BranchId)
            {
                return ApiResponse<ServiceRequestDto>.Failure("You are not authorized to cancel requests outside your assigned branch.", statusCode: 403);
            }
        }

        var requestEntity = await _context.ServiceRequests
            .FirstOrDefaultAsync(r => r.Id == command.ServiceRequestId, cancellationToken);

        if (requestEntity == null)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Service request not found.", statusCode: 404);
        }

        requestEntity.Status = RequestStatus.Cancelled;
        requestEntity.RejectionReason = string.IsNullOrWhiteSpace(command.Reason) ? "Request cancelled." : command.Reason;

        var historyRecord = new RequestStatusHistory
        {
            ServiceRequestId = requestEntity.Id,
            Status = RequestStatus.Cancelled,
            Note = $"Cancelled: {requestEntity.RejectionReason}",
            CreatedAt = DateTime.UtcNow
        };

        await _context.RequestStatusHistories.AddAsync(historyRecord, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var historyList = await _context.RequestStatusHistories
            .AsNoTracking()
            .Where(h => h.ServiceRequestId == requestEntity.Id)
            .OrderBy(h => h.CreatedAt)
            .Select(h => new RequestStatusHistoryDto
            {
                Id = h.Id,
                ServiceRequestId = h.ServiceRequestId,
                RequestNumber = requestData.RequestNumber,
                Status = h.Status,
                Note = h.Note,
                CreatedAt = h.CreatedAt,
                CreatedBy = h.CreatedBy
            })
            .ToListAsync(cancellationToken);

        var responseDto = new ServiceRequestDto
        {
            Id = requestEntity.Id,
            RequestNumber = requestData.RequestNumber,
            PersonId = requestData.PersonId,
            PersonFullName = requestData.PersonFullName,
            NationalNumber = requestData.NationalNumber,
            ServiceTypeId = requestData.ServiceTypeId,
            ServiceTypeName = requestData.ServiceTypeName,
            ServiceTypeCode = requestData.ServiceTypeCode,
            BranchId = requestData.BranchId,
            BranchName = requestData.BranchName,
            Status = requestEntity.Status,
            RejectionReason = requestEntity.RejectionReason,
            RequestDataJson = requestEntity.RequestDataJson,
            SubmissionDate = requestData.SubmissionDate,
            CompletedDate = requestEntity.CompletedDate,
            CreatedAt = requestEntity.CreatedAt,
            CreatedBy = requestEntity.CreatedBy,
            StatusHistory = historyList
        };

        return ApiResponse<ServiceRequestDto>.Success(responseDto, message: "Request cancelled successfully.");
    }
}
