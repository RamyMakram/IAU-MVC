using System;
using System.Collections.Generic;

#nullable disable

namespace TasahelAdmin.Models
{
    public partial class About
    {
        public int Id { get; set; }
        public int DomainId { get; set; }
        public string TitleAr { get; set; }
        public string TitleEn { get; set; }
        public string DescAr { get; set; }
        public string DescEn { get; set; }

        public virtual Domain Domain { get; set; }
    }
}
