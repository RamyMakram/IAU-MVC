using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitSignature
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; }
        public int UnitId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual Unit Unit { get; set; }
    }
}
