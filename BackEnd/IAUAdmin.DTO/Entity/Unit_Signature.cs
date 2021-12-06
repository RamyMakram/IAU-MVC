using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAUAdmin.DTO.Entity
{
    public class Unit_Signature
    {
        public string Path { get; set; }
        public string Base64 { get; set; }
        public string Name { get; set; }
        public int UnitID { get; set; }
        public List<HttpPostedFileBase> Files { get; set; }
    }
}
