using Huwiyati.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huwiyati.Domain.Events.Documents
{
    public class DocumentsExpiringEvent : BaseEvent
    {
        public Guid PersonId { get; set; }
        public string DocumentNumber {  get; set; } = string.Empty!;
        public DateOnly ExpirationDate { get; set; }
        public int DaysRemaining { get; set; }
        public DocumentsExpiringEvent(Guid personId, string documentNumber, DateOnly expirationDate, int daysRemaining)
        {
            PersonId = personId;
            DocumentNumber = documentNumber;
            ExpirationDate = expirationDate;
            DaysRemaining = daysRemaining;
        }


    }
}
