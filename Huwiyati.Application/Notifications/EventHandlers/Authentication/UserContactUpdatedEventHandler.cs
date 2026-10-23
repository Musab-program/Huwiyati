namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for creating user notifications when user contact details (email or phone number) are updated.
/// </summary>
public class UserContactUpdatedEventHandler : IDomainEventHandler<UserContactUpdatedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContactUpdatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public UserContactUpdatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="UserContactUpdatedEvent"/> by persisting an in-system notification.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing updated contact details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(UserContactUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        string title;
        string message;

        if (string.Equals(domainEvent.ContactType, "Email", StringComparison.OrdinalIgnoreCase))
        {
            title = "تحديث البريد الإلكتروني";
            message = $"تم تحديث بريدك الإلكتروني المسجل في نظام هويتي بنجاح إلى ({domainEvent.NewValue}).";
        }
        else
        {
            title = "تحديث رقم الهاتف";
            message = $"تم تحديث رقم هاتفك المسجل في نظام هويتي بنجاح إلى ({domainEvent.NewValue}).";
        }

        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
