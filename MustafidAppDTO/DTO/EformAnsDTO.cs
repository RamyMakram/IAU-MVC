using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class EformAnsDTO
    {
        public int EFAns_Q_ID { get; set; }
        public int EFAns_EF_ID { get; set; }
        public string EFAns_Value { get; set; }
        public string EFAns_Value_EN { get; set; }
        public ICollection<Preview_SavedTableColsDTO> EFAns_TableCol { get; set; }

    }
}
