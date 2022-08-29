using System;
using System.Collections.Generic;

#nullable disable

namespace MustafidAppModels.Models
{
    public partial class UnitsType
    {
        public UnitsType()
        {
            Units = new HashSet<Unit>();
        }

        public int UnitsTypeId { get; set; }
        public string UnitsTypeNameAr { get; set; }
        public string UnitsTypeNameEn { get; set; }
        public bool? IsAction { get; set; }
        public int? LevelId { get; set; }
        public string Code { get; set; }

        public virtual UnitLevel Level { get; set; }
        public virtual ICollection<Unit> Units { get; set; }
    }
}
