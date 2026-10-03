namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class AdminRestoredEventHandler : IDomainEventHandler<AdminRestoredEvent>
{
    private readonly IApplicationDbContext _context;

    public AdminRestoredEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(AdminRestoredEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إعادة تفعيل الحساب الإداري",
            Message = $"تم إعادة تفعيل حسابك الإداري لفرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
