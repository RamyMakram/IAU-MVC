using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class RequestData_DTO
	{
		public ApplicantRequest_Data_DTO Request { get; set; }
		public List<CustomeFile> Files { get; set; }
	}
}
