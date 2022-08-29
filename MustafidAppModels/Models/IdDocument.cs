using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class IdDocument
    {
        public IdDocument()
        {
            PersonelData = new HashSet<PersonelDatum>();
        }

        public int IdDocument1 { get; set; }
        public string DocumentNameEn { get; set; }
        public string DocumentNameAr { get; set; }
        public bool? IsAction { get; set; }

        public virtual ICollection<PersonelDatum> PersonelData { get; set; }
    }
}
