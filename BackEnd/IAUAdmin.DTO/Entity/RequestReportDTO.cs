using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class RequestReportDTO
	{
		public int Request_Data_ID { get; set; }
		public virtual ServiceTypeDTO Service_Type { get; set; }
		public virtual RequestTypeDTO Request_Type { get; set; }
		public virtual CountryDTO Country { get; set; }
		public virtual RequestStatusDTO Request_State { get; set; }
		public string Code_Generate { get; set; }
		public DateTime CreatedDate { get; set; }
		public string IAU_ID_Number { get; set; }
		public ApplicantTypeDTO Applicant_Type { get; set; }
		public string Middle_Name { get; set; }
		public string Last_Name { get; set; }
		public string ID_Number { get; set; }
		public virtual CountryDTO Country2 { get; set; }
		public virtual CityDTO City { get; set; }
		public virtual RegionDTO Region { get; set; }
		public string Postal_Code { get; set; }
		public string First_Name { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
	}
}
