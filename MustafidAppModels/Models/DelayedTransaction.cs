using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class DelayedTransaction
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int? DelayedOnUnitId { get; set; }
        public string RequestCode { get; set; }
        public byte RequestStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public int Delayed { get; set; }
        public DateTime AddedDate { get; set; }
        public bool Readed { get; set; }

        public virtual Unit DelayedOnUnit { get; set; }
        public virtual RequestDatum Request { get; set; }
        public virtual RequestState RequestStatusNavigation { get; set; }
    }
}
