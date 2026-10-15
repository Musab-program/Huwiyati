namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for alerting a user when a login attempt occurs from an unrecognized device.
/// </summary>
public class NewDeviceLoginAttemptEventHandler : IDomainEventHandler<NewDeviceLoginAttemptEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewDeviceLoginAttemptEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public NewDeviceLoginAttemptEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="NewDeviceLoginAttemptEvent"/> by persisting a security warning notification.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing device and user details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(NewDeviceLoginAttemptEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "محاولة دخول من جهاز جديد",
            Message = $"هناك محاولة تسجيل دخول لحسابك من جهاز جديد: {domainEvent.DeviceName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
