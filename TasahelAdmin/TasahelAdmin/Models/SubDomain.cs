using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class SubDomain
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string Key { get; set; }
        public string Domain { get; set; }
        public bool? UseHttps { get; set; }

        public virtual Domain DomainNavigation { get; set; }
    }
}
