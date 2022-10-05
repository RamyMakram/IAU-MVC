using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class TasahelHomeSetting
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string NewReqIco { get; set; }
        public string FollowIco { get; set; }
        public bool EnableAcadamic { get; set; }

        public virtual Domain Domain { get; set; }
    }
}
