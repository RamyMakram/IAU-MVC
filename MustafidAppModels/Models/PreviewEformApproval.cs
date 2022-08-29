using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class PreviewEformApproval
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int UnitId { get; set; }
        public bool OwnEform { get; set; }
        public DateTime? SignDate { get; set; }
        public int PersonEform { get; set; }

        public virtual PersonEform PersonEformNavigation { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
