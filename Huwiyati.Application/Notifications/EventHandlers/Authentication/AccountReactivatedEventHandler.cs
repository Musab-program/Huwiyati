namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for creating user notifications when an account is reactivated.
/// </summary>
public class AccountReactivatedEventHandler : IDomainEventHandler<AccountReactivatedEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="AccountReactivatedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public AccountReactivatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="AccountReactivatedEvent"/> by persisting an in-app notification for the user.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing reactivated account details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(AccountReactivatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إعادة تفعيل الحساب",
            Message = "تم إعادة تفعيل حسابك الرقمي بنجاح.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
