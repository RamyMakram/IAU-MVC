using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UnitLevelDTO
	{
		public int ID { get; set; }
		public string Name_AR { get; set; }
		public string Name_EN { get; set; }
		public string Code { get; set; }

		public virtual ICollection<UnitsDTO> Units { get; set; }
		public virtual ICollection<UnitTypeDTO> Units_Type { get; set; }
		public virtual string Units_Type_STR { get; set; }
	}
}
