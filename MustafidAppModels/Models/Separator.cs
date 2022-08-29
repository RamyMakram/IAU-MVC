using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class Separator
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public bool IsEmpty { get; set; }

        public virtual Question Question { get; set; }
    }
}
