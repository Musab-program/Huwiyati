namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class AdminRemovedEventHandler : IDomainEventHandler<AdminRemovedEvent>
{
    private readonly IApplicationDbContext _context;

    public AdminRemovedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(AdminRemovedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "إعفاء من صلاحيات الأدمن",
            Message = $"تم إعفاؤك من صلاحيات المدير لفرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
