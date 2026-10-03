using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.DTOs;
using Huwiyati.Domain.Entities.Requests;
using Huwiyati.Domain.Enums;
using Microsoft.EntityFrameworkCore;

using Huwiyati.Domain.Events.Requests;

namespace Huwiyati.Application.Requests.Commands;

// Handler responsible for modifying ServiceRequest status with branch security checks
public class ChangeServiceRequestStatusHandler
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;
    private readonly IDomainEventHandler<ServiceRequestStatusChangedEvent> _statusChangedEventHandler;

    public ChangeServiceRequestStatusHandler(
        IApplicationDbContext context,
        IIdentityService identityService,
        IDomainEventHandler<ServiceRequestStatusChangedEvent> statusChangedEventHandler)
    {
        _context = context;
        _identityService = identityService;
        _statusChangedEventHandler = statusChangedEventHandler;
    }

    public async Task<ApiResponse<ServiceRequestDto>> ChangeStatusAsync(
        ChangeServiceRequestStatusCommand command,
        Guid currentEmployeeUserId,
        CancellationToken cancellationToken = default)
    {
        // 1. Fetch Request data using Select (NO Include)
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

        // Check finalized state guards
        if (requestData.Status == RequestStatus.Cancelled)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Cannot change status of a cancelled request.", statusCode: 400);
        }

        if (requestData.Status == RequestStatus.Issued)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Cannot change status of an already issued request.", statusCode: 400);
        }

        // 2. Employee Branch Authorization Check: Ensure employee belongs to target processing branch
        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.UserId == currentEmployeeUserId && e.IsActive, cancellationToken);

        if (employee == null || employee.BranchId != requestData.BranchId)
        {
            return ApiResponse<ServiceRequestDto>.Failure(
                "You are not authorized to process requests outside your assigned branch.", statusCode: 403);
        }

        // 3. Fetch tracked entity for mutation
        var requestEntity = await _context.ServiceRequests
            .FirstOrDefaultAsync(r => r.Id == command.ServiceRequestId, cancellationToken);

        if (requestEntity == null)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Service request not found.", statusCode: 404);
        }

        // 4. Update status (ID and RequestNumber remain unchanged)
        requestEntity.Status = command.NewStatus;
        if (command.NewStatus == RequestStatus.Rejected)
        {
            requestEntity.RejectionReason = command.RejectionReason;
        }

        if (command.NewStatus == RequestStatus.Issued)
        {
            requestEntity.CompletedDate = DateTime.UtcNow;
        }

        // 5. Append new status history record
        var historyRecord = new RequestStatusHistory
        {
            ServiceRequestId = requestEntity.Id,
            Status = command.NewStatus,
            Note = command.Note ?? (command.NewStatus == RequestStatus.Rejected ? command.RejectionReason : $"Status updated to {command.NewStatus}"),
            CreatedAt = DateTime.UtcNow
        };

        await _context.RequestStatusHistories.AddAsync(historyRecord, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // 5.1 Trigger Domain Event if person has an active user account
        var userId = await _identityService.GetUserIdByPersonIdAsync(requestData.PersonId, cancellationToken);
        if (userId != null)
        {
            var statusEvent = new ServiceRequestStatusChangedEvent(userId.Value, requestData.RequestNumber, command.NewStatus.ToString());
            await _statusChangedEventHandler.HandleAsync(statusEvent, cancellationToken);
        }

        // 6. Map and return response using Select-fetched history records
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

        return ApiResponse<ServiceRequestDto>.Success(responseDto, message: "Request status updated successfully.");
    }
}
