using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class ServiceType
    {
        public ServiceType()
        {
            MainServices = new HashSet<MainService>();
            RequestData = new HashSet<RequestDatum>();
            UnitServiceTypes = new HashSet<UnitServiceType>();
            Units = new HashSet<Unit>();
        }

        public int ServiceTypeId { get; set; }
        public string ServiceTypeNameEn { get; set; }
        public string ServiceTypeNameAr { get; set; }
        public bool? IsAction { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }
        public string ImagePath { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<MainService> MainServices { get; set; }
        public virtual ICollection<RequestDatum> RequestData { get; set; }
        public virtual ICollection<UnitServiceType> UnitServiceTypes { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
