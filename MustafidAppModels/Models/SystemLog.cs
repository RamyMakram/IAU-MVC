using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class SystemLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Method { get; set; }
        public DateTime TransDate { get; set; }
        public int ClassType { get; set; }
        public string Oldval { get; set; }
        public string Newval { get; set; }
        public string CallPath { get; set; }
        public int? ReferId { get; set; }
        public string Notes { get; set; }

        public virtual User User { get; set; }
    }
}
