using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasahelAdmin.Models.VM
{
    public class DomainCreateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain1 { get; set; }
        public string ConnectionString { get; set; }
        public bool Enabled { get; set; }
        public DateTime EndDate { get; set; }

        public DomainStyleVM DomainStyle { get; set; }
        public HomeVM HomeSettings { get; set; }
        public DomainInfo DomainInfo { get; set; }
        public DomainEmail DomainEmail { get; set; }

        public virtual ICollection<About> Abouts { get; set; }
        public virtual ICollection<SubDomain> SubDomains { get; set; }
    }
}
