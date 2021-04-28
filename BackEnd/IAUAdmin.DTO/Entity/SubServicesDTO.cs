using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class SubServicesDTO
	{
		public int Sub_Services_ID { get; set; }
		public string Sub_Services_Name_EN { get; set; }
		public string Sub_Services_Name_AR { get; set; }
		public Nullable<int> Main_Services_ID { get; set; }
		public Nullable<bool> IS_Action { get; set; }
	}
}
