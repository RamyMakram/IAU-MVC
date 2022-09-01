using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppDTO.DTO
{
    public partial class SeparatorDTO
    {
        public int Sepa_ID { get; set; }
        public bool Sepa_Empty { get; set; }/*If True => Empty Sapce , other borderd separtor*/
    }
}
