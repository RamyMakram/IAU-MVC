using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class DomainStyle
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string Favicon { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeyword { get; set; }
        public string Maincolor { get; set; }
        public string Secondcolor { get; set; }
        public string Thirdcolor { get; set; }

        public virtual Domain Domain { get; set; }
    }
}
