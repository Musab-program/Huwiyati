namespace Huwiyati.Infrastructure.BackgroundJobs;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Events.Documents;
using Huwiyati.Application.Notifications.EventHandlers.Passport;
using Huwiyati.Application.Notifications.EventHandlers.Documents;
using Huwiyati.Application.Notifications.EventHandlers.Family;
using Huwiyati.Domain.Enums;

public class DocumentExpirationMonitorJob : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DocumentExpirationMonitorJob> _logger;

    // Standard global milestone thresholds (in days)
    private static readonly int[] PassportMilestones = { 180, 90, 30, 7, 1 };
    private static readonly int[] NationalIdMilestones = { 90, 60, 30, 7, 1 };
    private static readonly int[] FamilyCardMilestones = { 90, 60, 30, 7, 1 };

    public DocumentExpirationMonitorJob(
        IServiceScopeFactory scopeFactory,
        ILogger<DocumentExpirationMonitorJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Document Expiration Monitor Job started.");

        // Execute immediately upon startup, then repeat daily
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckDocumentExpirationsAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while monitoring document expirations.");
            }

            // Wait 24 hours for the next execution
            using var timer = new PeriodicTimer(TimeSpan.FromHours(24));
            await timer.WaitForNextTickAsync(stoppingToken);
        }
    }

    private async Task CheckDocumentExpirationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

        var passportHandler = scope.ServiceProvider.GetRequiredService<PassportExpiringEventHandler>();
        var nationalIdHandler = scope.ServiceProvider.GetRequiredService<NationalIdCardExpiringEventHandler>();
        var familyCardHandler = scope.ServiceProvider.GetRequiredService<FamilyCardExpiringEventHandler>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // 1. Process Passports
        var expiredPassports = await context.Passports
            .Where(p => p.Status == PassportStatus.Active && p.ExpiryDate <= today)
            .ToListAsync(cancellationToken);

        foreach (var passport in expiredPassports)
        {
            passport.Status = PassportStatus.Expired;
        }

        var activePassports = await context.Passports
            .Where(p => p.Status == PassportStatus.Active && p.ExpiryDate > today)
            .ToListAsync(cancellationToken);

        foreach (var passport in activePassports)
        {
            var daysRemaining = passport.ExpiryDate.DayNumber - today.DayNumber;
            if (PassportMilestones.Contains(daysRemaining))
            {
                var evt = new DocumentsExpiringEvent(passport.PersonId, passport.PassportNumber, passport.ExpiryDate, daysRemaining);
                await passportHandler.HandleAsync(evt, cancellationToken);
            }
        }

        // 2. Process National ID Cards
        var expiredNationalIds = await context.NationalIdCards
            .Where(c => c.Status == NationalIdCardStatus.Active && c.ExpiryDate <= today)
            .ToListAsync(cancellationToken);

        foreach (var card in expiredNationalIds)
        {
            card.Status = NationalIdCardStatus.Expired;
        }

        var activeNationalIds = await context.NationalIdCards
            .Where(c => c.Status == NationalIdCardStatus.Active && c.ExpiryDate > today)
            .ToListAsync(cancellationToken);

        foreach (var card in activeNationalIds)
        {
            var daysRemaining = card.ExpiryDate.DayNumber - today.DayNumber;
            if (NationalIdMilestones.Contains(daysRemaining))
            {
                var person = await context.Persons.FirstOrDefaultAsync(p => p.Id == card.PersonId, cancellationToken);
                var docNumber = person != null ? person.NationalNumber : card.Id.ToString();

                var evt = new DocumentsExpiringEvent(card.PersonId, docNumber, card.ExpiryDate, daysRemaining);
                await nationalIdHandler.HandleAsync(evt, cancellationToken);
            }
        }

        // 3. Process Family Cards
        var expiredFamilies = await context.Families
            .Where(f => f.Status == FamilyStatus.Active && f.ExpiryDate <= today)
            .ToListAsync(cancellationToken);

        foreach (var family in expiredFamilies)
        {
            family.Status = FamilyStatus.Expired;
        }

        var activeFamilies = await context.Families
            .Where(f => f.Status == FamilyStatus.Active && f.ExpiryDate > today)
            .ToListAsync(cancellationToken);

        foreach (var family in activeFamilies)
        {
            var daysRemaining = family.ExpiryDate.DayNumber - today.DayNumber;
            if (FamilyCardMilestones.Contains(daysRemaining))
            {
                var evt = new DocumentsExpiringEvent(family.HeadOfFamilyPersonId, family.FamilyNumber, family.ExpiryDate, daysRemaining);
                await familyCardHandler.HandleAsync(evt, cancellationToken);
            }
        }

        await context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Document Expiration Check completed successfully.");
    }
}
