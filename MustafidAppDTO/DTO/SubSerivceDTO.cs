using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class SubSerivceDTO
    {
        public int SS_ID { get; set; }
        public string SS_Name { get; set; }
        public string SS_Name_EN { get; set; }
        public IEnumerable<RequiredDocsDTO> SS_Docs { get; set; }
    }
}
