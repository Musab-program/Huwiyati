namespace Huwiyati.Application.Notifications.EventHandlers.Passport;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Passport;

// Handler responsible for creating a notification when a Travel Record is added to a Passport
public class TravelRecordAddedEventHandler : IDomainEventHandler<TravelRecordAddedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public TravelRecordAddedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    public async Task HandleAsync(TravelRecordAddedEvent domainEvent, CancellationToken cancellationToken = default)
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
            Title = "تسجيل حركة سفر جديدة",
            Message = $"تم تسجيل حركة سفر جديدة لجواز سفرك برقم {domainEvent.PassportNumber} (الدولة: {domainEvent.Country}).",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Persist notification record to database
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
