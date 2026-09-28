namespace Huwiyati.Infrastructure.Services;

using Huwiyati.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

public class RequestNumberGenerator : IRequestNumberGenerator
{
    private readonly IApplicationDbContext _context;

    public RequestNumberGenerator(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> GenerateRequestNumberAsync(CancellationToken cancellationToken = default)
    {
        var datePrefix = DateTime.UtcNow.ToString("yyyyMMdd");
        var prefix = $"REQ-{datePrefix}-";

        // Find the count of requests created today to generate sequential number
        var todayCount = await _context.ServiceRequests
            .CountAsync(r => r.RequestNumber.StartsWith(prefix), cancellationToken);

        var nextSequence = (todayCount + 1).ToString("D4");

        return $"{prefix}{nextSequence}";
    }
}
