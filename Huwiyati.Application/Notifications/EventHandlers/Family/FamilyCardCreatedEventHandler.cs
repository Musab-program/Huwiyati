namespace Huwiyati.Application.Notifications.EventHandlers.Family;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Family;

public class FamilyCardCreatedEventHandler : IDomainEventHandler<FamilyCardCreatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public FamilyCardCreatedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task HandleAsync(FamilyCardCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.HeadOfFamilyPersonId, cancellationToken);
        if (userId == null) return;

        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "إصدار بطاقة عائلية",
            Message = $"تم إصدار بطاقة عائلية جديدة لأسرتك بنجاح برقم {domainEvent.FamilyNumber}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
