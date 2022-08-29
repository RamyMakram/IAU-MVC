using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class TitleMiddleName
    {
        public TitleMiddleName()
        {
            PersonelData = new HashSet<PersonelDatum>();
        }

        public int TitleMiddleNamesId { get; set; }
        public string TitleMiddleNamesNameEn { get; set; }
        public string TitleMiddleNamesNameAr { get; set; }
        public bool? IsAction { get; set; }

        public virtual ICollection<PersonelDatum> PersonelData { get; set; }
    }
}
