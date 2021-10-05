using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class JobDTO
	{
		public int? User_Permissions_Type_ID { get; set; }
		public string User_Permissions_Type_Name_AR { get; set; }
		public string User_Permissions_Type_Name_EN { get; set; }
		public string Name { get; set; }
		public virtual ICollection<PrivilgesDTO> Permissions { get; set; }
		public virtual ICollection<Job_PermissionsDTO> Job_Permissions { get; set; }
	}
}
