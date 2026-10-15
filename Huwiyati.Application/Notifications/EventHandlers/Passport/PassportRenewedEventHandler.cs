namespace Huwiyati.Application.Notifications.EventHandlers.Passport;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Passport;

/// <summary>
/// Handles <see cref="PassportRenewedEvent"/> by creating an in-app notification when an existing passport is renewed.
/// </summary>
public class PassportRenewedEventHandler : IDomainEventHandler<PassportRenewedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="PassportRenewedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity service to resolve user identifiers.</param>
    public PassportRenewedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the passport renewed event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(PassportRenewedEvent domainEvent, CancellationToken cancellationToken = default)
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
            Title = "تم تجديد جواز السفر",
            Message = $"تم تجديد جواز سفرك بنجاح برقم {domainEvent.PassportNumber}. تاريخ الانتهاء الجديد: {domainEvent.NewExpiryDate}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        // 3. Persist notification record to database
        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
