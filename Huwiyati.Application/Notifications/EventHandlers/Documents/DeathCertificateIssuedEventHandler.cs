namespace Huwiyati.Application.Notifications.EventHandlers.Documents;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;
using Microsoft.EntityFrameworkCore;

public class DeathCertificateIssuedEventHandler : IDomainEventHandler<DeathCertificateIssuedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public DeathCertificateIssuedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

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
