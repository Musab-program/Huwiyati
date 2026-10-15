using Huwiyati.Application.Common.Interfaces;
using Huwiyati.Domain.Entities.Notifications;
using Huwiyati.Domain.Events.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Application.Notifications.EventHandlers.Passport
{
    /// <summary>
    /// Handles <see cref="DocumentsExpiringEvent"/> specifically for expiring passports by dispatching an alert notification.
    /// </summary>
    public class PassportExpiringEventHandler : IDomainEventHandler<DocumentsExpiringEvent>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassportExpiringEventHandler"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="identityService">The identity service to resolve user identifiers.</param>
        public PassportExpiringEventHandler(
            IApplicationDbContext context,
            IIdentityService identityService)
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
            var userId = await _identityService.GetUserIdByPersonIdAsync(domainEvent.PersonId);
            if (userId == null) return;

            var notification = new Notification
            {
                UserId = userId.Value,
                Title = "تنبيه قرب انتهاء جواز السفر",
                Message = $"نود إشعاراتكم بأن جواز السفر رقم ({domainEvent.DocumentNumber}) سيغدو منتهي الصلاحية خلال {domainEvent.DaysRemaining} يوماً (بتاريخ {domainEvent.ExpirationDate:yyyy/MM/dd}). يرجى تقديم طلب تجديد الجواز عبر الخدمة الذاتية.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Notifications.AddAsync(notification , cancellationToken);
            await _context.SaveChangesAsync();

        }
    }
}
