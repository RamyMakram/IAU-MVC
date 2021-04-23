using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UnitsDTO
	{
		public int Units_ID { get; set; }
		public string Units_Name_AR { get; set; }
		public string Units_Name_EN { get; set; }
		public Nullable<int> Units_Location_ID { get; set; }
		public Nullable<int> Units_Type_ID { get; set; }
		public Nullable<int> ServiceType_ID { get; set; }
		public string Ref_Number { get; set; }
		public string Building_Number { get; set; }
		public Nullable<bool> IS_Action { get; set; }
		public bool IS_Mostafid { get; set; }
		public int[] Units_ReqType { get; set; }


		public virtual ICollection<Units_Request_TypeDTO> Units_Request_Type { get; set; }
		public virtual ICollection<RequestTypeDTO> Request_Type { get; set; }
		public virtual UnitsLocDTO Units_Location { get; set; }
		public virtual UnitTypeDTO Units_Type { get; set; }
		public virtual ServiceTypeDTO Service_Type { get; set; }

	}
}
