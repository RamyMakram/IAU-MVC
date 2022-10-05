using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class DomainInfo
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Address { get; set; }

        public virtual Domain Domain { get; set; }
    }
}
