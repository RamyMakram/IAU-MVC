using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class EForm
    {
        public EForm()
        {
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public int SubServiceId { get; set; }
        public bool? IsAction { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Code { get; set; }
        public int UnitToApprove { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual SubService SubService { get; set; }
        public virtual Unit UnitToApproveNavigation { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
