namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for creating user notifications when a password reset is requested.
/// </summary>
public class PasswordResetRequestedEventHandler : IDomainEventHandler<PasswordResetRequestedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetRequestedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public PasswordResetRequestedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="PasswordResetRequestedEvent"/> by creating an in-app security notification.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing password reset details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(PasswordResetRequestedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "طلب إعادة تعيين كلمة المرور",
            Message = "تم تقديم طلب لإعادة تعيين كلمة المرور الخاصة بحسابك.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
