using System.Collections.Generic;

namespace MustafidAppDTO.DTO
{
    public class Preview_TableColsDTO
    {
        public int TC_ID { get; set; }//ColumnID
        public virtual ICollection<Tables_AnswareDTO> TC_Answare { get; set; }
    }
}