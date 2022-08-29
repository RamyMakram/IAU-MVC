using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RadioType
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }

        public virtual Question Question { get; set; }
    }
}
