using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Country
    {
        public Country()
        {
            PersonelDatumAddressCountries = new HashSet<PersonelDatum>();
            PersonelDatumCountries = new HashSet<PersonelDatum>();
            PersonelDatumNationalities = new HashSet<PersonelDatum>();
            Regions = new HashSet<Region>();
        }

        public int CountryId { get; set; }
        public string CountryNameEn { get; set; }
        public string CountryNameAr { get; set; }
        public bool? IsAction { get; set; }
        public int Index { get; set; }

        public virtual ICollection<PersonelDatum> PersonelDatumAddressCountries { get; set; }
        public virtual ICollection<PersonelDatum> PersonelDatumCountries { get; set; }
        public virtual ICollection<PersonelDatum> PersonelDatumNationalities { get; set; }
        public virtual ICollection<Region> Regions { get; set; }
    }
}
