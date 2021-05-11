using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class Unit_MainServiceEditDTO
	{
		public List<UnitMainServicesDTO> Added { get; set; }
		public List<UnitMainServicesDTO> Deleted { get; set; }
	}
}
