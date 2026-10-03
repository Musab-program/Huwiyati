namespace Huwiyati.Application.Notifications.EventHandlers.Documents;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;

public class DeathCertificateUpdatedEventHandler : IDomainEventHandler<DeathCertificateUpdatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public DeathCertificateUpdatedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task HandleAsync(DeathCertificateUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.RelativePersonId, cancellationToken);
        if (userId == null) return;

        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "تحديث بيانات شهادة الوفاة",
            Message = $"تم تحديث بيانات شهادة الوفاة الخاصة بـ {domainEvent.DeceasedName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
