namespace Huwiyati.Infrastructure.BackgroundJobs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;

public class VerificationCodeCleanupJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<VerificationCodeCleanupJob> _logger;

    public VerificationCodeCleanupJob(
        IServiceScopeFactory scopeFactory,
        ILogger<VerificationCodeCleanupJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Verification Code Cleanup Job started.");

        // Execute immediately upon startup, then repeat daily
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredCodesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while cleaning up expired verification codes.");
            }

            // Wait 24 hours for the next execution
            using var timer = new PeriodicTimer(TimeSpan.FromHours(24));
            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }

    private async Task CleanupExpiredCodesAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var now = DateTime.UtcNow;

        var expiredCodes = await context.VerificationCodes
            .Where(vc => vc.ExpirationTime < now || vc.IsUsed)
            .ToListAsync(cancellationToken);

        if (expiredCodes.Any())
        {
            context.VerificationCodes.RemoveRange(expiredCodes);
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Cleaned up {Count} expired or used verification codes.", expiredCodes.Count);
        }
    }
}
