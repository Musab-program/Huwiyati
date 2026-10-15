namespace Huwiyati.Application.Notifications.EventHandlers.Documents;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Event handler for broadcasting notifications to surviving family members upon issuance of a death certificate.
/// </summary>
public class DeathCertificateIssuedEventHandler : IDomainEventHandler<DeathCertificateIssuedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeathCertificateIssuedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity service to resolve user accounts from person records.</param>
    public DeathCertificateIssuedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the <see cref="DeathCertificateIssuedEvent"/> by notifying registered family members of the deceased.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing deceased person details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(DeathCertificateIssuedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        // 1. Find family records linked to the deceased person
        var familyIds = await _context.FamilyMembers
            .Where(fm => fm.PersonId == domainEvent.DeceasedPersonId)
            .Select(fm => fm.FamilyId)
            .ToListAsync(cancellationToken);

        if (familyIds.Count > 0)
        {
            // 2. Fetch all other active family members in the family
            var familyMemberPersonIds = await _context.FamilyMembers
                .Where(fm => familyIds.Contains(fm.FamilyId) && fm.PersonId != domainEvent.DeceasedPersonId)
                .Select(fm => fm.PersonId)
                .Distinct()
                .ToListAsync(cancellationToken);

            // 3. Send notification to each family member with an active user account
            foreach (var personId in familyMemberPersonIds)
            {
                var userId = await _identityService.GetUserIdByPersonIdAsync(personId, cancellationToken);
                if (userId != null)
                {
                    var notification = new Notification
                    {
                        UserId = userId.Value,
                        Title = "توثيق شهادة وفاة",
                        Message = $"تم توثيق وإصدار شهادة وفاة رسمية للمرحوم/ة {domainEvent.DeceasedName}.",
                        IsRead = false,
                        CreatedAt = now
                    };
                    await _context.Notifications.AddAsync(notification, cancellationToken);
                }
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
