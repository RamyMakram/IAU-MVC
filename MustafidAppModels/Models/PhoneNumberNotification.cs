using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class PhoneNumberNotification
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public string MessageEn { get; set; }
        public int? RequestId { get; set; }
        public DateTime NotiDate { get; set; }
        public int? UserId { get; set; }
        public bool Readed { get; set; }

        public virtual RequestDatum Request { get; set; }
        public virtual User User { get; set; }
    }
}
