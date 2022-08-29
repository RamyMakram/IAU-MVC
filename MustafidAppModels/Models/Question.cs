using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Question
    {
        public Question()
        {
            CheckBoxTypes = new HashSet<CheckBoxType>();
            EFormsAnswers = new HashSet<EFormsAnswer>();
            RadioTypes = new HashSet<RadioType>();
            TableColumns = new HashSet<TableColumn>();
        }

        public int Id { get; set; }
        public int IndexOrder { get; set; }
        public int EformId { get; set; }
        public string Type { get; set; }
        public string LableName { get; set; }
        public string LableNameEn { get; set; }
        public bool? Requird { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RefTo { get; set; }
        public int? TableRowsNum { get; set; }
        public bool? Active { get; set; }

        public virtual EForm Eform { get; set; }
        public virtual InputType InputType { get; set; }
        public virtual Paragraph Paragraph { get; set; }
        public virtual Separator Separator { get; set; }
        public virtual ICollection<CheckBoxType> CheckBoxTypes { get; set; }
        public virtual ICollection<EFormsAnswer> EFormsAnswers { get; set; }
        public virtual ICollection<RadioType> RadioTypes { get; set; }
        public virtual ICollection<TableColumn> TableColumns { get; set; }
    }
}
