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
		public Nullable<DateTime> DeletedAt { get; set; }
		public string Required { get; set; }
		public ICollection<RequiredDocsDTO> Required_Documents { get; set; }
		public virtual MainServiceDTO Main_Services { get; set; }

	}
}
