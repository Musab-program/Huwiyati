namespace Huwiyati.Application.Notifications.EventHandlers.Organizations;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Organizations;

public class EmployeeActivatedEventHandler : IDomainEventHandler<EmployeeActivatedEvent>
{
    private readonly IApplicationDbContext _context;

    public EmployeeActivatedEventHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(EmployeeActivatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var notification = new Notification
        {
            UserId = domainEvent.UserId,
            Title = "تفعيل حساب الموظف",
            Message = $"تم إعادة تفعيل حساب الموظف الخاص بك في فرع {domainEvent.BranchName}.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
