using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequestType
    {
        public RequestType()
        {
            RequestData = new HashSet<RequestDatum>();
            UnitsRequestTypes = new HashSet<UnitsRequestType>();
        }

        public int RequestTypeId { get; set; }
        public string RequestTypeNameEn { get; set; }
        public string RequestTypeNameAr { get; set; }
        public bool? IsAction { get; set; }
        public string DescEn { get; set; }
        public string DescAr { get; set; }
        public string ImagePath { get; set; }
        public bool IsRequestType { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<RequestDatum> RequestData { get; set; }
        public virtual ICollection<UnitsRequestType> UnitsRequestTypes { get; set; }
    }
}
