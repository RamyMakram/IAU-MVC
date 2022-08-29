using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitLevel
    {
        public UnitLevel()
        {
            Units = new HashSet<Unit>();
            UnitsTypes = new HashSet<UnitsType>();
        }

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Unit> Units { get; set; }
        public virtual ICollection<UnitsType> UnitsTypes { get; set; }
    }
}
