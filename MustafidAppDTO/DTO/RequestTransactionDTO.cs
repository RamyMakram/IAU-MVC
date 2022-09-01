using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class RequestTransactionDTO
    {
        public DateTime? ReqTrans_DateWillEnd { get; set; }
        public UnitsDTO ReqTrans_Unit { get; set; }
    }
}
