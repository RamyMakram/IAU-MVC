using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class MainService
    {
        public MainService()
        {
            SubServices = new HashSet<SubService>();
            UnitMainServices = new HashSet<UnitMainService>();
            ValidTos = new HashSet<ValidTo>();
        }

        public int MainServicesId { get; set; }
        public string MainServicesNameEn { get; set; }
        public string MainServicesNameAr { get; set; }
        public bool? IsAction { get; set; }
        public int? ServiceTypeId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ServiceType ServiceType { get; set; }
        public virtual ICollection<SubService> SubServices { get; set; }
        public virtual ICollection<UnitMainService> UnitMainServices { get; set; }
        public virtual ICollection<ValidTo> ValidTos { get; set; }
    }
}
