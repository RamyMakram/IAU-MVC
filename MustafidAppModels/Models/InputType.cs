using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class InputType
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Placeholder { get; set; }
        public string PlaceholderEn { get; set; }
        public bool? IsNumber { get; set; }
        public bool IsDate { get; set; }

        public virtual Question Question { get; set; }
    }
}
