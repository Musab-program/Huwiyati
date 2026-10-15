using Huwiyati.Application.Common;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Application.Common.Models;
using Huwiyati.Application.Requests.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Huwiyati.Application.Requests.Queries;

/// <summary>
/// Handles querying paginated service requests submitted by a specific citizen.
/// </summary>
public class GetCitizenServiceRequestsHandler
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetCitizenServiceRequestsHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public GetCitizenServiceRequestsHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves a paginated list of service requests submitted by the specified citizen.
    /// </summary>
    /// <param name="query">The pagination and filtering parameters.</param>
    /// <param name="currentPersonId">The person identifier of the citizen.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An <see cref="ApiResponse{T}"/> containing a <see cref="PaginatedList{T}"/> of <see cref="ServiceRequestDto"/> items.</returns>
    public async Task<ApiResponse<PaginatedList<ServiceRequestDto>>> GetCitizenRequestsAsync(
        GetCitizenServiceRequestsQuery query,
        Guid currentPersonId,
        CancellationToken cancellationToken = default)
    {
        if (currentPersonId == Guid.Empty)
        {
            return ApiResponse<PaginatedList<ServiceRequestDto>>.Failure("Invalid or unassociated Person ID.", statusCode: 400);
        }

        var baseQuery = _context.ServiceRequests
            .AsNoTracking()
            .Where(r => r.PersonId == currentPersonId);

        if (query.Status.HasValue)
        {
            baseQuery = baseQuery.Where(r => r.Status == query.Status.Value);
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
            paginatedResult, message: "Citizen service requests retrieved successfully.", statusCode: 200);
    }
}
