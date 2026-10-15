using Huwiyati.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Domain.Events.Documents;

using Huwiyati.Domain.Common;

/// <summary>
/// Domain event raised when an official document (such as a national ID or passport) is approaching its expiration date.
/// </summary>
public class DocumentsExpiringEvent : BaseEvent
{
    /// <summary>
    /// Gets or sets the unique identifier of the citizen holding the document.
    /// </summary>
    public Guid PersonId { get; set; }

    /// <summary>
    /// Gets or sets the document serial or identification number.
    /// </summary>
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date on which the document expires.
    /// </summary>
    public DateOnly ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the count of days remaining until expiration.
    /// </summary>
    public int DaysRemaining { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DocumentsExpiringEvent"/> class.
    /// </summary>
    /// <param name="personId">The unique identifier of the citizen.</param>
    /// <param name="documentNumber">The document serial or card number.</param>
    /// <param name="expirationDate">The expiration date.</param>
    /// <param name="daysRemaining">The number of days remaining until expiration.</param>
    public DocumentsExpiringEvent(Guid personId, string documentNumber, DateOnly expirationDate, int daysRemaining)
    {
        PersonId = personId;
        DocumentNumber = documentNumber;
        ExpirationDate = expirationDate;
        DaysRemaining = daysRemaining;
    }
}
