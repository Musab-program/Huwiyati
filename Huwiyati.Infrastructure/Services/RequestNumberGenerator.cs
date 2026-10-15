namespace Huwiyati.Infrastructure.Services;

using Huwiyati.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Service responsible for generating unique sequential service request tracking numbers formatted with daily date prefixes.
/// </summary>
public class RequestNumberGenerator : IRequestNumberGenerator
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestNumberGenerator"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public RequestNumberGenerator(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Generates a unique request tracking number for the current date.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A formatted tracking number string.</returns>
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
