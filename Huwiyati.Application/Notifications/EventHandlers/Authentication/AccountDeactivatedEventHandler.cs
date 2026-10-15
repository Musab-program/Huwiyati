namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for creating user notifications when an account is deactivated.
/// </summary>
public class AccountDeactivatedEventHandler : IDomainEventHandler<AccountDeactivatedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountDeactivatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public AccountDeactivatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="AccountDeactivatedEvent"/> by persisting an in-app notification for the user.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing deactivated account details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(AccountDeactivatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إلغاء تفعيل الحساب",
            Message = "تم إلغاء تفعيل حسابك الرقمي في تطبيق هويتي.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
