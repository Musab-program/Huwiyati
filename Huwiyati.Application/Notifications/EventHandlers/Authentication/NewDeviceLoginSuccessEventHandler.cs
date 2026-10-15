namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for creating user notifications when a new device successfully logs into an account.
/// </summary>
public class NewDeviceLoginSuccessEventHandler : IDomainEventHandler<NewDeviceLoginSuccessEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="NewDeviceLoginSuccessEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public NewDeviceLoginSuccessEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="NewDeviceLoginSuccessEvent"/> by persisting an informational notification.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing device and user details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(NewDeviceLoginSuccessEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تسجيل دخول من جهاز جديد",
            Message = $"تم تسجيل الدخول بنجاح لحسابك من جهاز جديد: {domainEvent.DeviceName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
