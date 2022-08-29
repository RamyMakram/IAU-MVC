using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class City
    {
        public City()
        {
            PersonelData = new HashSet<PersonelDatum>();
        }

        public int CityId { get; set; }
        public string CityNameAr { get; set; }
        public string CityNameEn { get; set; }
        public int? RegionId { get; set; }
        public bool? IsAction { get; set; }

        public virtual Region Region { get; set; }
        public virtual ICollection<PersonelDatum> PersonelData { get; set; }
    }
}
