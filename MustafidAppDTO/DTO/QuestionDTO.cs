using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppDTO.DTO
{
    public partial class QuestionDTO
    {

        public int Q_ID { get; set; }
        public int Q_Order { get; set; }
        public string Q_Type { get; set; }
        public string Q_Lable_Name { get; set; }
        public string Q_Lable_Name_En { get; set; }
        public bool? Q_Is_Req { get; set; }
        public string Q_RefName { get; set; }
        public int? Q_Rows_Num { get; set; }

        public virtual InputTypeDTO Q_Input { get; set; }
        public virtual ParagraphDTO Q_Para { get; set; }
        public virtual SeparatorDTO Q_Sep { get; set; }
        public virtual ICollection<CheckBoxTypeDTO> Q_Check_Box { get; set; }
        public virtual ICollection<RadioTypeDTO> Q_Radio { get; set; }
        public virtual ICollection<TableColumnDTO> Q_T_Columns { get; set; }
    }
}
