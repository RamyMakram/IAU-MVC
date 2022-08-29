using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitMainService
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        public int MainServiceId { get; set; }

        public virtual MainService MainService { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
