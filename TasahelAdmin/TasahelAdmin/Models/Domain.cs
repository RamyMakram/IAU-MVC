using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class Domain
    {
        public Domain()
        {
            Abouts = new HashSet<About>();
            SubDomains = new HashSet<SubDomain>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain1 { get; set; }
        public string ConnectionString { get; set; }
        public string DomainKey { get; set; }
        public string DomainMachineId { get; set; }
        public bool Enabled { get; set; }
        public DateTime EndDate { get; set; }

        public virtual DomainEmail DomainEmail { get; set; }
        public virtual DomainInfo DomainInfo { get; set; }
        public virtual DomainStyle DomainStyle { get; set; }
        public virtual TasahelHomeSetting TasahelHomeSetting { get; set; }
        public virtual ICollection<About> Abouts { get; set; }
        public virtual ICollection<SubDomain> SubDomains { get; set; }
    }
}
