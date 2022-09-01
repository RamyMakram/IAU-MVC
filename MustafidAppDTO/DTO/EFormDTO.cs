using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppDTO.DTO
{
    public partial class EFormDTO
    {
        public int EF_ID { get; set; }
        public string EF_Name { get; set; }
        public string EF_Name_EN { get; set; }
        public string EF_Code { get; set; }

        public virtual UnitsDTO EF_Approved_Unit { get; set; }
        public virtual ICollection<QuestionDTO> EF_Q_List { get; set; }
    }
}
