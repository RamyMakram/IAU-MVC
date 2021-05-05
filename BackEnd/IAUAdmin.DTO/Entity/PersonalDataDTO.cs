using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class PersonalDataDTO
	{
		public int Personel_Data_ID { get; set; }
		public int ID_Document { get; set; }
		public string ID_Number { get; set; }
		public Nullable<int> IAU_Affiliate_ID { get; set; }
		public string IAU_ID_Number { get; set; }
		public Nullable<int> Applicant_Type_ID { get; set; }
		public Nullable<int> Title_Middle_Names_ID { get; set; }
		public string First_Name { get; set; }
		public string Middle_Name { get; set; }
		public string Last_Name { get; set; }
		public Nullable<int> Nationality_ID { get; set; }
		public Nullable<int> Country_ID { get; set; }
		public string City_Country_1 { get; set; }
		public string City_Country_2 { get; set; }
		public string Region_Postal_Code_1 { get; set; }
		public string Region_Postal_Code_2 { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
		public string IS_Action { get; set; }
	}
}
