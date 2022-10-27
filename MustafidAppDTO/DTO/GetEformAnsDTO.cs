using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class GetEformAnsDTO : EformAnsDTO
    {
        public int Order { get; set; }
        public string EFAns_T { get; set; }
        public string EFAns_Name_EN { get; set; }
        public string EFAns_Name { get; set; }
    }
}
