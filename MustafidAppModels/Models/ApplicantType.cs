using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class ApplicantType
    {
        public ApplicantType()
        {
            PersonelData = new HashSet<PersonelDatum>();
            ValidTos = new HashSet<ValidTo>();
        }

        public int ApplicantTypeId { get; set; }
        public string ApplicantTypeNameEn { get; set; }
        public string ApplicantTypeNameAr { get; set; }
        public bool? IsAction { get; set; }
        public bool Affliated { get; set; }
        public int Index { get; set; }

        public virtual ICollection<PersonelDatum> PersonelData { get; set; }
        public virtual ICollection<ValidTo> ValidTos { get; set; }
    }
}
