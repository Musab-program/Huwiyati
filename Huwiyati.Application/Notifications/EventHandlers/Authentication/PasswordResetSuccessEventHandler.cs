namespace Huwiyati.Application.Notifications.EventHandlers.Authentication;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Authentication;

public class PasswordResetSuccessEventHandler : IDomainEventHandler<PasswordResetSuccessEvent>
{
    private readonly IApplicationDbContext _context;

    public PasswordResetSuccessEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

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
