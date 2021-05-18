using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UnitTypeDTO
	{
		public int Units_Type_ID { get; set; }
		public string Units_Type_Name_AR { get; set; }
		public string Units_Type_Name_EN { get; set; }
		public string Name { get; set; }
		public Nullable<bool> IS_Action { get; set; }
		public string Code { get; set; }

		public virtual ICollection<UnitsDTO> Units { get; set; }
		public virtual UnitLevelDTO UnitLevel { get; set; }
	}
}
