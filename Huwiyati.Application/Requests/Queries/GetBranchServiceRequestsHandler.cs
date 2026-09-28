using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Application.Requests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Requests.Queries;

// Handler responsible for querying requests assigned to the logged-in employee's branch
public class GetBranchServiceRequestsHandler
{
    private readonly IApplicationDbContext _context;

    public GetBranchServiceRequestsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<PaginatedList<ServiceRequestDto>>> GetBranchRequestsAsync(
        GetBranchServiceRequestsQuery query,
        string employeeUserId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(employeeUserId) || !Guid.TryParse(employeeUserId, out var userIdGuid))
        {
            return ApiResponse<PaginatedList<ServiceRequestDto>>.Failure("Invalid employee identity in security context.", statusCode: 401);
        }

        // Resolve assigned branch for employee
        var employeeBranchInfo = await _context.Employees
            .AsNoTracking()
            .Where(e => e.UserId == userIdGuid && e.IsActive)
            .Select(e => new { e.BranchId, e.Branch.IsActive })
            .FirstOrDefaultAsync(cancellationToken);

        if (employeeBranchInfo == null)
        {
            return ApiResponse<PaginatedList<ServiceRequestDto>>.Failure("Employee record or assigned branch was not found.", statusCode: 404);
        }

        if (!employeeBranchInfo.IsActive)
        {
            return ApiResponse<PaginatedList<ServiceRequestDto>>.Failure("Employee assigned branch is currently inactive.", statusCode: 400);
        }

        var baseQuery = _context.ServiceRequests
            .AsNoTracking()
            .Where(r => r.BranchId == employeeBranchInfo.BranchId);

        if (query.Status.HasValue)
        {
            baseQuery = baseQuery.Where(r => r.Status == query.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(query.SearchKeyword))
        {
            var keyword = query.SearchKeyword.Trim();
            baseQuery = baseQuery.Where(r => r.RequestNumber.Contains(keyword) || r.Person.NationalNumber.Contains(keyword));
        }

        var projectedQuery = baseQuery
            .OrderByDescending(r => r.SubmissionDate)
            .Select(r => new ServiceRequestDto
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
            });

        var pageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
        var pageSize = query.PageSize < 1 ? 10 : query.PageSize;

        var paginatedResult = await PaginatedList<ServiceRequestDto>.CreateAsync(
            projectedQuery,
            pageNumber,
            pageSize
        );

        return ApiResponse<PaginatedList<ServiceRequestDto>>.Success(
            paginatedResult, message: "Branch service requests retrieved successfully.", statusCode: 200);
    }
}
