namespace Huwiyati.Application.Notifications.EventHandlers.Passport;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Passport;

/// <summary>
/// Handles <see cref="TravelRecordAddedEvent"/> by creating an in-app notification when a travel movement is logged.
/// </summary>
public class TravelRecordAddedEventHandler : IDomainEventHandler<TravelRecordAddedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="TravelRecordAddedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity service to resolve user identifiers.</param>
    public TravelRecordAddedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the travel record added event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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
