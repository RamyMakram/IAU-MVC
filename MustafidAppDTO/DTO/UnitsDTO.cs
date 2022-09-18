using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MustafidAppDTO.DTO
{
    public class UnitsDTO
    {
        [AllowNull]
        public int? U_ID { get; set; }
        public string U_Name { get; set; }
        public string U_Name_EN { get; set; }
    }
}
