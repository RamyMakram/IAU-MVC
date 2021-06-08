using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAU.DTO.Entity
{
	public class MainServiceDTO
	{
		public int Main_Services_ID { get; set; }
		public string Main_Services_Name_EN { get; set; }
		public string Main_Services_Name_AR { get; set; }
		public Nullable<bool> IS_Action { get; set; }
		public Nullable<int> ServiceTypeID { get; set; }
		public int[] Applicant_Types { get; set; }
		public ICollection<ApplicantTypeDTO> MainService_ApplicantType { get; set; }
		public bool Active { get; set; }

		public virtual ServiceTypeDTO Service_Type { get; set; }
		public virtual ICollection<SubServicesDTO> Sub_Services { get; set; }
	}
}
