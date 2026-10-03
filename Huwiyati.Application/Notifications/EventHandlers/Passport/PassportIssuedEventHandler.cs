namespace Huwiyati.Application.Notifications.EventHandlers.Passport;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Passport;

// Handler responsible for creating a notification when a Passport is issued
public class PassportIssuedEventHandler : IDomainEventHandler<PassportIssuedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public PassportIssuedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task HandleAsync(PassportIssuedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // 1. Resolve UserId from PersonId using IdentityService
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.PersonId, cancellationToken);
        if (userId == null)
        {
            // Person does not have an active user account yet, skip creating notification
            return;
        }

        // 2. Instantiate new Notification entity
        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "تم إصدار جواز السفر",
            Message = $"تم إصدار جواز سفرك بنجاح برقم {domainEvent.PassportNumber}. تاريخ الانتهاء: {domainEvent.ExpiryDate}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Persist notification record to database
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
