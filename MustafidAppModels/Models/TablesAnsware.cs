using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class TablesAnsware
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Value { get; set; }

        public virtual PreviewTableCol ColumnNavigation { get; set; }
    }
}
