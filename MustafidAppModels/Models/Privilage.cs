using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Privilage
    {
        public Privilage()
        {
            InverseDetailedFromNavigation = new HashSet<Privilage>();
            InverseSubOfNavigation = new HashSet<Privilage>();
            JobPermissions = new HashSet<JobPermission>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? SubOf { get; set; }
        public int? DetailedFrom { get; set; }

        public virtual Privilage DetailedFromNavigation { get; set; }
        public virtual Privilage SubOfNavigation { get; set; }
        public virtual ICollection<Privilage> InverseDetailedFromNavigation { get; set; }
        public virtual ICollection<Privilage> InverseSubOfNavigation { get; set; }
        public virtual ICollection<JobPermission> JobPermissions { get; set; }
    }
}
