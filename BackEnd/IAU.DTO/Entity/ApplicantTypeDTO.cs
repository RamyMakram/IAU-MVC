using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class ApplicantTypeDTO
	{
		public int? Applicant_Type_ID { get; set; }
		public string Applicant_Type_Name_EN { get; set; }
		public string Applicant_Type_Name_AR { get; set; }
		public Nullable<bool> IS_Action { get; set; }
	}
}
