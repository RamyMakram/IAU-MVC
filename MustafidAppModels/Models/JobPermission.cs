using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class JobPermission
    {
        public int Id { get; set; }
        public int? PrivilageId { get; set; }
        public int? JobId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Job Job { get; set; }
        public virtual Privilage Privilage { get; set; }
    }
}
