using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppDTO.DTO
{
    public partial class InputTypeDTO
    {
        public int IT_ID { get; set; }
        public string IT_Desc { get; set; }
        public string IT_Desc_EN { get; set; }
        public bool? IT_Num_Only { get; set; }
        public bool IT_Date_Only { get; set; }
    }
}
