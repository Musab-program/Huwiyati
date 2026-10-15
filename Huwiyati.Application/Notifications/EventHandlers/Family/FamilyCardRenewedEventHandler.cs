namespace Huwiyati.Application.Notifications.EventHandlers.Family;

using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Family;

/// <summary>
/// Handles <see cref="FamilyCardRenewedEvent"/> by dispatching an in-app confirmation notification to the head of the family.
/// </summary>
public class FamilyCardRenewedEventHandler : IDomainEventHandler<FamilyCardRenewedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FamilyCardRenewedEventHandler"/> class.
    /// </summary>
    /// <param name="context">The application database context.</param>
    /// <param name="identityService">The identity service to resolve user identifiers.</param>
    public FamilyCardRenewedEventHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        _identityService = identityService;
    }

    /// <summary>
    /// Handles the family card renewed event asynchronously.
    /// </summary>
    /// <param name="domainEvent">The domain event payload.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task HandleAsync(FamilyCardRenewedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.HeadOfFamilyPersonId, cancellationToken);
        if (userId == null) return;

        var notification = new Notification
        {
            UserId = userId.Value,
            Title = "تجديد البطاقة العائلية",
            Message = $"تم تجديد البطاقة العائلية الخاصة بأسرتك بنجاح.",
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Notifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
