using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class EFormsAnswer
    {
        public EFormsAnswer()
        {
            PreviewTableCols = new HashSet<PreviewTableCol>();
        }

        public int Id { get; set; }
        public int? QuestionId { get; set; }
        public int? EformId { get; set; }
        public string Value { get; set; }
        public string ValueEn { get; set; }
        public DateTime? FillDate { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string NameEn { get; set; }
        public int? IndexOrder { get; set; }

        public virtual PersonEform Eform { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<PreviewTableCol> PreviewTableCols { get; set; }
    }
}
