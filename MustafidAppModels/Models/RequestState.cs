using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequestState
    {
        public RequestState()
        {
            DelayedTransactions = new HashSet<DelayedTransaction>();
            RequestData = new HashSet<RequestDatum>();
            RequestLogs = new HashSet<RequestLog>();
        }

        public byte StateId { get; set; }
        public string StateNameAr { get; set; }
        public string StateNameEn { get; set; }
        public byte? AllowedDelay { get; set; }

        public virtual ICollection<DelayedTransaction> DelayedTransactions { get; set; }
        public virtual ICollection<RequestDatum> RequestData { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
    }
}
