using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequestLog
    {
        public byte RequestStateId { get; set; }
        public int Id { get; set; }
        public int RequestId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual RequestDatum Request { get; set; }
        public virtual RequestState RequestState { get; set; }
    }
}
