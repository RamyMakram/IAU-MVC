using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequiredDocument
    {
        public RequiredDocument()
        {
            RequestFiles = new HashSet<RequestFile>();
        }

        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public int? SubServiceId { get; set; }
        public bool? IsAction { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletetAt { get; set; }

        public virtual SubService SubService { get; set; }
        public virtual ICollection<RequestFile> RequestFiles { get; set; }
    }
}
