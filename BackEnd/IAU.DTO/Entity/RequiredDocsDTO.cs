using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class RequiredDocsDTO
	{
		public int? ID { get; set; }
		public string Name_EN { get; set; }
		public string Name_AR { get; set; }
		public Nullable<int> SubServiceID { get; set; }
		public Nullable<bool> IS_Action { get; set; }
	}
}
