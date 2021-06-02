using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAUAdmin.DTO.Entity
{
	public class RequestData_DTO
	{
		public ApplicantRequest_Data_DTO Request { get; set; }
		public List<HttpPostedFileBase> Files { get; set; }
	}
	public class CustomeFile
	{
		public byte[] bytes { get; set; }
		public string filename { get; set; }
	}
}
