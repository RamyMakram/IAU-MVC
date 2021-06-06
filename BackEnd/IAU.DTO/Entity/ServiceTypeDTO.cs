using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAU.DTO.Entity
{
	public class ServiceTypeDTO
	{
		public int Service_Type_ID { get; set; }
		public string Service_Type_Name_EN { get; set; }
		public string Service_Type_Name_AR { get; set; }
		public string Name { get; set; }
		public Nullable<bool> IS_Action { get; set; }
		public string Image_Path { get; set; }
	}
}
