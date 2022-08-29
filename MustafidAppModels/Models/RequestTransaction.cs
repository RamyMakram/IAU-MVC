using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class RequestTransaction
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int FromUnitId { get; set; }
        public int? ToUnitId { get; set; }
        public DateTime? ForwardDate { get; set; }
        public DateTime? ExpireDays { get; set; }
        public string Comment { get; set; }
        public int? CommentType { get; set; }
        public bool Readed { get; set; }
        public DateTime? ReadedDate { get; set; }
        public string MostafidComment { get; set; }
        public DateTime? CommentDate { get; set; }
        public string Code { get; set; }
        public bool IsReminder { get; set; }

        public virtual Unit FromUnit { get; set; }
        public virtual RequestDatum Request { get; set; }
        public virtual Unit ToUnit { get; set; }
    }
}
