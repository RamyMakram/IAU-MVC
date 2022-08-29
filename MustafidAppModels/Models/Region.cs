using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Region
    {
        public Region()
        {
            Cities = new HashSet<City>();
            PersonelData = new HashSet<PersonelDatum>();
        }

        public int RegionId { get; set; }
        public string RegionNameAr { get; set; }
        public string RegionNameEn { get; set; }
        public int CountryId { get; set; }
        public bool? IsAction { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<PersonelDatum> PersonelData { get; set; }
    }
}
