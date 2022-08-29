using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class SubService
    {
        public SubService()
        {
            EForms = new HashSet<EForm>();
            RequestData = new HashSet<RequestDatum>();
            RequiredDocuments = new HashSet<RequiredDocument>();
        }

        public int SubServicesId { get; set; }
        public string SubServicesNameEn { get; set; }
        public string SubServicesNameAr { get; set; }
        public int? MainServicesId { get; set; }
        public bool? IsAction { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual MainService MainServices { get; set; }
        public virtual ICollection<EForm> EForms { get; set; }
        public virtual ICollection<RequestDatum> RequestData { get; set; }
        public virtual ICollection<RequiredDocument> RequiredDocuments { get; set; }
    }
}
