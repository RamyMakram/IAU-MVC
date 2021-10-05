using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class LocationsDTO
	{
		public int Location_ID { get; set; }
		public string Location_Name_AR { get; set; }
		public string Location_Name_EN { get; set; }
		public string Name { get; set; }
		public Nullable<bool> IS_Action { get; set; }
	}
}
