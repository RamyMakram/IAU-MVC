﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class PersonalDataDTO
	{
		public int Personel_Data_ID { get; set; }
		public string IAU_ID_Number { get; set; }
		public Nullable<int> Applicant_Type_ID { get; set; }
		public Nullable<int> Title_Middle_Names_ID { get; set; }
		public string First_Name { get; set; }
		public string Middle_Name { get; set; }
		public string Last_Name { get; set; }
		public Nullable<int> Nationality_ID { get; set; }
		public int? ID_Document { get; set; }
		public Nullable<int> Country_ID { get; set; }
		public string ID_Number { get; set; }
		public int Address_CountryID { get; set; }
		public Nullable<int> Address_CityID { get; set; }
		public Nullable<int> Adress_RegionID { get; set; }
		public string Address_City { get; set; }
		public string Adress_Region { get; set; }
		public string Postal_Code { get; set; }
		public string Email { get; set; }
		public string Mobile { get; set; }
		public string IS_Action { get; set; }
		public ApplicantTypeDTO Applicant_Type { get; set; }
		public virtual IDDocDTO ID_Document1 { get; set; }
		public virtual ICollection<E_Forms_Answer> Answer { get; set; }
		public virtual ICollection<PersonEfDTO> Person_Eform { get; set; }
		public virtual CityDTO City { get; set; }
		public virtual RegionDTO Region { get; set; }
		public virtual CountryDTO Country { get; set; }
		public virtual CountryDTO Country1 { get; set; }
		public virtual CountryDTO Country2 { get; set; }
	}
}
