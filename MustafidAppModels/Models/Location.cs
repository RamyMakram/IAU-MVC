using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Location
    {
        public Location()
        {
            UnitsLocations = new HashSet<UnitsLocation>();
        }

        public int LocationId { get; set; }
        public string LocationNameAr { get; set; }
        public string LocationNameEn { get; set; }
        public bool? IsAction { get; set; }

        public virtual ICollection<UnitsLocation> UnitsLocations { get; set; }
    }
}
