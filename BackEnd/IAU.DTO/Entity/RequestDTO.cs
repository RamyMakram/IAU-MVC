using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace IAU.DTO.Entity
{
	public class RequestData_DTO
	{
		public ApplicantRequest_Data_DTO Request { get; set; }
		public List<HttpPostedFileBase> Files { get; set; }
	}
}
