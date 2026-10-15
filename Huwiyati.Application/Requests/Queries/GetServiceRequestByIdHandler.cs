using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Requests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Requests.Queries;

/// <summary>
/// Handles querying a single service request by its unique identifier, enforcing access control permissions.
/// </summary>
public class GetServiceRequestByIdHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetServiceRequestByIdHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetServiceRequestByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a service request by its unique identifier asynchronously.
    /// </summary>
    /// <param name="requestId">The service request unique identifier.</param>
    /// <param name="currentPersonId">Optional person identifier if called by a citizen.</param>
    /// <param name="currentEmployeeUserId">Optional employee user identifier if called by branch staff.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing the resulting <see cref="ServiceRequestDto"/>.</returns>
    public async Task<ApiResponse<ServiceRequestDto>> GetByIdAsync(
        Guid requestId,
        Guid? currentPersonId = null,
        string? currentEmployeeUserId = null,
        CancellationToken cancellationToken = default)
    {
        if (requestId == Guid.Empty)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Invalid request identifier.", statusCode: 400);
        }

        var record = await _context.ServiceRequests
            .AsNoTracking()
            .Where(r => r.Id == requestId)
            .Select(r => new
            {
                Dto = new ServiceRequestDto
                {
                    Id = r.Id,
                    RequestNumber = r.RequestNumber,
                    PersonId = r.PersonId,
                    PersonFullName = (r.Person.FirstName + " " + r.Person.FatherName + " " + r.Person.GrandfatherName + " " + r.Person.FamilyName).Trim(),
                    NationalNumber = r.Person.NationalNumber,
                    ServiceTypeId = r.ServiceTypeId,
                    ServiceTypeName = r.ServiceType.Name,
                    ServiceTypeCode = r.ServiceType.Code,
                    BranchId = r.BranchId,
                    BranchName = r.Branch.BranchName,
                    Status = r.Status,
                    RejectionReason = r.RejectionReason,
                    RequestDataJson = r.RequestDataJson,
                    SubmissionDate = r.SubmissionDate,
                    CompletedDate = r.CompletedDate,
                    CreatedAt = r.CreatedAt,
                    CreatedBy = r.CreatedBy,
                    StatusHistory = r.StatusHistory
                        .OrderByDescending(h => h.CreatedAt)
                        .Select(h => new RequestStatusHistoryDto
                        {
                            Id = h.Id,
                            ServiceRequestId = h.ServiceRequestId,
                            RequestNumber = r.RequestNumber,
                            Status = h.Status,
                            Note = h.Note,
                            CreatedAt = h.CreatedAt,
                            CreatedBy = h.CreatedBy
                        }).ToList()
                },
                r.PersonId,
                r.BranchId
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (record == null)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Service request record was not found.", statusCode: 404);
        }

        // Verify citizen ownership if currentPersonId is provided
        if (currentPersonId.HasValue && currentPersonId.Value != Guid.Empty && record.PersonId != currentPersonId.Value)
        {
            return ApiResponse<ServiceRequestDto>.Failure("Unauthorized. You can only view service requests belonging to you.", statusCode: 403);
        }

        // Verify employee branch matching if currentEmployeeUserId is provided
        if (!string.IsNullOrWhiteSpace(currentEmployeeUserId) && Guid.TryParse(currentEmployeeUserId, out var employeeUserIdGuid))
        {
            var employeeBranchId = await _context.Employees
                .AsNoTracking()
                .Where(e => e.UserId == employeeUserIdGuid && e.IsActive)
                .Select(e => (Guid?)e.BranchId)
                .FirstOrDefaultAsync(cancellationToken);

            if (!employeeBranchId.HasValue || employeeBranchId.Value != record.BranchId)
            {
                return ApiResponse<ServiceRequestDto>.Failure("Unauthorized. You can only view service requests submitted to your assigned branch.", statusCode: 403);
            }
        }

        return ApiResponse<ServiceRequestDto>.Success(record.Dto, message: "Service request retrieved successfully.", statusCode: 200);
    }
}
