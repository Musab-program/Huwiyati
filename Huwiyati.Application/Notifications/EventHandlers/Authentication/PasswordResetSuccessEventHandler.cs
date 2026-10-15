namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

/// <summary>
/// Event handler for creating user notifications when a password has been successfully reset.
/// </summary>
public class PasswordResetSuccessEventHandler : IDomainEventHandler<PasswordResetSuccessEvent>
{
    private readonly IApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordResetSuccessEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    public PasswordResetSuccessEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Handles the <see cref="PasswordResetSuccessEvent"/> by persisting a confirmation notification.
    /// </summary>
    /// <param name="domainEvent">The domain event instance containing user details.</param>
    /// <param name="cancellationToken">A token to observe while waiting for the operation to complete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(PasswordResetSuccessEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تم تغيير كلمة المرور",
            Message = "تم تغيير كلمة المرور الخاصة بحسابك بنجاح.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
