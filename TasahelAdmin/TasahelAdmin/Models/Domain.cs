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
            DomainInfos = new HashSet<DomainInfo>();
            TasahelHomeSettings = new HashSet<TasahelHomeSetting>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Domain1 { get; set; }
        public string ConnectionString { get; set; }
        public string Favicon { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeyword { get; set; }
        public string Maincolor { get; set; }
        public string Secondcolor { get; set; }
        public string Thirdcolor { get; set; }
        public string DomainKey { get; set; }
        public string DomainMachineId { get; set; }
        public bool Enabled { get; set; }
        public DateTime EndDate { get; set; }

        public virtual ICollection<About> Abouts { get; set; }
        public virtual ICollection<DomainInfo> DomainInfos { get; set; }
        public virtual ICollection<TasahelHomeSetting> TasahelHomeSettings { get; set; }
    }
}
