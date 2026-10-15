using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Notifications.EventHandlers.Family
{
    /// <summary>
    /// Handles <see cref="DocumentsExpiringEvent"/> specifically for expiring family record cards by notifying the card owner.
    /// </summary>
    public class FamilyCardExpiringEventHandler : IDomainEventHandler<DocumentsExpiringEvent>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FamilyCardExpiringEventHandler"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="identityService">The identity service to resolve user identifiers.</param>
        public FamilyCardExpiringEventHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        /// <summary>
        /// Handles the document expiration event asynchronously.
        /// </summary>
        /// <param name="domainEvent">The domain event payload.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task HandleAsync(DocumentsExpiringEvent domainEvent, CancellationToken cancellationToken = default)
        {
            var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.PersonId, cancellationToken);
            if (userId == null) return;
            var notification = new Notification
            {
                UserId = userId.Value,
                Title = "تنبيه قرب انتهاء السجل الأسري",
                Message = $"نود إشعاراتكم بأن البطاقة العائلية رقم ({domainEvent.DocumentNumber}) ستنتهي خلال {domainEvent.DaysRemaining} يوماً (بتاريخ {domainEvent.ExpirationDate:yyyy/MM/dd}). يرجى مراجعة مكاتب الأحوال المدنية أو تقديم طلب التجديد.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Notifications.AddAsync(notification, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}