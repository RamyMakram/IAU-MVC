using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class PersonEform
    {
        public PersonEform()
        {
            EFormsAnswers = new HashSet<EFormsAnswer>();
            PreviewEformApprovals = new HashSet<PreviewEformApproval>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int? PersonId { get; set; }
        public int? RequestId { get; set; }
        public string Code { get; set; }
        public DateTime FillDate { get; set; }

        public virtual PersonelDatum Person { get; set; }
        public virtual RequestDatum Request { get; set; }
        public virtual ICollection<EFormsAnswer> EFormsAnswers { get; set; }
        public virtual ICollection<PreviewEformApproval> PreviewEformApprovals { get; set; }
    }
}
