using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitServiceType
    {
        public int Id { get; set; }
        public int ServiceTypeId { get; set; }
        public int UnitId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ServiceType ServiceType { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
