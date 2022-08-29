using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitsRequestType
    {
        public int UnitsRequestTypeId { get; set; }
        public int? UnitsId { get; set; }
        public int? RequestTypeId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual RequestType RequestType { get; set; }
        public virtual Unit Units { get; set; }
    }
}
