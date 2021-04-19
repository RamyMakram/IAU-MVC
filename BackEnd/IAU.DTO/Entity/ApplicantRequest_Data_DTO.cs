using Model.DTO;
using System;
using System.Collections.Generic;
using System.Web;

namespace IAU.DTO.Entity
{
	public class ApplicantRequest_Data_DTO
	{

		public Nullable<int> Affiliated { get; set; }
		public string Affiliated_Name { get; set; }
		public string IAUID { get; set; }

		public Nullable<int> Applicant_Type_ID { get; set; }
		public string Applicant_Type_Name { get; set; }

		public Nullable<int> title { get; set; }

		public Nullable<int> Nationality_ID { get; set; }
		public string Nationality_Name { get; set; }


		public Nullable<int> Country_ID { get; set; }
		public string Country_Name { get; set; }

		public Nullable<int> ID_Document { get; set; }
		public string ID_Document_Name { get; set; }
		public string Document_Number { get; set; }

		public Nullable<int> provider { get; set; }
		public string provider_Name { get; set; }

		public Nullable<int> Main_Services_ID { get; set; }
		public string Main_Services_Name { get; set; }

		public Nullable<int> Sub_Services_ID { get; set; }
		public string Sub_Services_Name { get; set; }

		public Nullable<int> Supporting_Documents_ID { get; set; }
		public string Mobile { get; set; }
		public string Email { get; set; }
		public string Required_Fields_Notes { get; set; }

		public HttpPostedFile filelist { get; set; }
		public string Region_Postal_Code_1 { get; set; }
		public string Region_Postal_Code_2 { get; set; }
		public string City_Country_1 { get; set; }
		public string City_Country_2 { get; set; }

		public Nullable<int> Request_Type_ID { get; set; } = 1;
		public string Request_Type_Name { get; set; }

		public Nullable<int> Service_Type_ID { get; set; } = 1;
		public string Service_Type_Name { get; set; }

		public string filePath { get; set; }
		public List<SelectListItemDto> serviceTypeList { get; set; }
		public List<SelectListItemDto> requestTypeList { get; set; }

		public string Name { get; set; }
		public string Address { get; set; }
		public string Region { get; set; }
		public string postal { get; set; }
		public string First_Name { get; set; }
		public string Middle_Name { get; set; }
		public string Last_Name { get; set; }
		public string ID_Number { get; set; }
		public string Title_Middle_Names { get; set; }
		public List<string> file_names { get; set; }
	}
}
