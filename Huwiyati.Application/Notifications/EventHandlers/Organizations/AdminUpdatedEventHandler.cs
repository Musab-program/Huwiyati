namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class AdminUpdatedEventHandler : IDomainEventHandler<AdminUpdatedEvent>
{
    private readonly IApplicationDbContext _context;

    public AdminUpdatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(AdminUpdatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تحديث صلاحيات أدمن",
            Message = $"تم تحديث بيانات وصلاحيات الحساب الإداري الخاص بك في فرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
