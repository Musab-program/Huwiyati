namespace Huwiyati.Application.Notifications.EventHandlers.Documents;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;

public class NationalIdCardIssuedEventHandler : IDomainEventHandler<NationalIdCardIssuedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public NationalIdCardIssuedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task HandleAsync(NationalIdCardIssuedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.PersonId, cancellationToken);
        if (userId == null) return;

        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "تم إصدار البطاقة الشخصية",
            Message = $"تم إصدار بطاقتك الشخصية الجديدة بنجاح برقم {domainEvent.CardNumber}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
