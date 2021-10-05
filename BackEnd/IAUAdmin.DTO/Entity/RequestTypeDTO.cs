using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAUAdmin.DTO.Entity
{
	public class RequestTypeDTO
	{
		public int Request_Type_ID { get; set; }
		public string Request_Type_Name_EN { get; set; }
		public string Request_Type_Name_AR { get; set; }
		public string Name { get; set; }
		public Nullable<bool> IS_Action { get; set; }
		public List<HttpPostedFileBase> Files { get; set; }
		public string Image_Path { get; set; }
		public string Base64 { get; set; }
	}
}
