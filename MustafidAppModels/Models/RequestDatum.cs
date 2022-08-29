using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequestDatum
    {
        public RequestDatum()
        {
            DelayedTransactions = new HashSet<DelayedTransaction>();
            PersonEforms = new HashSet<PersonEform>();
            PhoneNumberNotifications = new HashSet<PhoneNumberNotification>();
            RequestFiles = new HashSet<RequestFile>();
            RequestLogs = new HashSet<RequestLog>();
            RequestTransactions = new HashSet<RequestTransaction>();
        }

        public int RequestDataId { get; set; }
        public int? PersonelDataId { get; set; }
        public int? UnitId { get; set; }
        public int? SubServicesId { get; set; }
        public string RequiredFieldsNotes { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? RequestTypeId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CodeGenerate { get; set; }
        public string TempCode { get; set; }
        public byte RequestStateId { get; set; }
        public bool? IsTwasulOc { get; set; }
        public bool? Readed { get; set; }
        public DateTime? ReadedDate { get; set; }
        public DateTime? GenratedDate { get; set; }
        public bool IsArchived { get; set; }

        public virtual PersonelDatum PersonelData { get; set; }
        public virtual RequestState RequestState { get; set; }
        public virtual RequestType RequestType { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual SubService SubServices { get; set; }
        public virtual Unit Unit { get; set; }
        public virtual ICollection<DelayedTransaction> DelayedTransactions { get; set; }
        public virtual ICollection<PersonEform> PersonEforms { get; set; }
        public virtual ICollection<PhoneNumberNotification> PhoneNumberNotifications { get; set; }
        public virtual ICollection<RequestFile> RequestFiles { get; set; }
        public virtual ICollection<RequestLog> RequestLogs { get; set; }
        public virtual ICollection<RequestTransaction> RequestTransactions { get; set; }
    }
}
