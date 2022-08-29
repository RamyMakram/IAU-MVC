using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class PreviewTableCol
    {
        public PreviewTableCol()
        {
            TablesAnswares = new HashSet<TablesAnsware>();
        }

        public int Id { get; set; }
        public int EformAnswareId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }

        public virtual EFormsAnswer EformAnsware { get; set; }
        public virtual ICollection<TablesAnsware> TablesAnswares { get; set; }
    }
}
