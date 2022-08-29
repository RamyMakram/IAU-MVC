using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitsLocation
    {
        public UnitsLocation()
        {
            Units = new HashSet<Unit>();
        }

        public int UnitsLocationId { get; set; }
        public string UnitsLocationNameAr { get; set; }
        public string UnitsLocationNameEn { get; set; }
        public int? LocationId { get; set; }
        public bool? IsAction { get; set; }
        public string Code { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Location Location { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
