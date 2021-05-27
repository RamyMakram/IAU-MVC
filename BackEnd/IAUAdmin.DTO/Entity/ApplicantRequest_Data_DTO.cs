using System;
using System.Collections.Generic;
using System.Web;

namespace IAUAdmin.DTO.Entity
{
	public class ApplicantRequest_Data_DTO
	{
		public int Request_Data_ID { get; set; }
		public Nullable<int> Personel_Data_ID { get; set; }
		public Nullable<int> Provider_Academic_Services_ID { get; set; }
		public Nullable<int> Sub_Services_ID { get; set; }
		public string Required_Fields_Notes { get; set; }
		public Nullable<int> Service_Type_ID { get; set; }
		public Nullable<int> Request_Type_ID { get; set; }
		public System.DateTime CreatedDate { get; set; }
		public string Code_Generate { get; set; }
		public byte Request_State_ID { get; set; }
		public string signpdfpath { get; set; }
		public Nullable<bool> IsTwasul_OC { get; set; }

		public virtual PersonalDataDTO Personel_Data { get; set; }
	}
}
