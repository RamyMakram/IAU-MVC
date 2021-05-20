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
		public string Ref_Number { get; set; }
		public string Building_Number { get; set; }
		public Nullable<bool> IS_Action { get; set; }
		public bool IS_Mostafid { get; set; }
		public Nullable<int> LevelID { get; set; }
		public Nullable<int> SubID { get; set; }
		public Nullable<int> ServiceTypeID { get; set; }
		public int[] Units_ReqType { get; set; }
		public int[] Units_ServiceType { get; set; }
		public string Code { get; set; }


		public virtual ICollection<UnitServiceTypesDTO> UnitServiceTypes { get; set; }
		public virtual ICollection<UnitMainServicesDTO> UnitMainServices { get; set; }

		public virtual ICollection<Units_Request_TypeDTO> Units_Request_Type { get; set; }
		public virtual ICollection<RequestTypeDTO> Request_Type { get; set; }
		public virtual UnitsLocDTO Units_Location { get; set; }
		public virtual UnitTypeDTO Units_Type { get; set; }
		public virtual ServiceTypeDTO Service_Type { get; set; }
		public virtual UnitLevelDTO UnitLevel { get; set; }
		public virtual ICollection<UnitsDTO> Units1 { get; set; }
		public virtual UnitsDTO Units2 { get; set; }

		public virtual ICollection<ServiceTypeDTO> ServiceTypes { get; set; }
		public virtual ICollection<MainServiceDTO> MainServices { get; set; }

	}
}
