using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
    public class LogDTO
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Method { get; set; }
        public System.DateTime TransDate { get; set; }
        public int ClassType { get; set; }
        public string Oldval { get; set; }
        public string Newval { get; set; }
        public string CallPath { get; set; }
        public Nullable<int> ReferID { get; set; }
        public string Notes { get; set; }
        public string Message { get; set; }

        public virtual UserDTO Users { get; set; }
    }
}
