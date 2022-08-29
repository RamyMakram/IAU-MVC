using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequestFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int RequestId { get; set; }
        public int? RequiredDocId { get; set; }

        public virtual RequestDatum Request { get; set; }
        public virtual RequiredDocument RequiredDoc { get; set; }
    }
}
