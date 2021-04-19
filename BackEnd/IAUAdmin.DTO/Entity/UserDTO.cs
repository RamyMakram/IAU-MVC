using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class UserDTO
	{
		public int User_ID { get; set; }
		public string User_Name { get; set; }
		public string User_Mobile { get; set; }
		public string User_Email { get; set; }
		public string User_Password { get; set; }
		public Nullable<int> Job_ID { get; set; }
		public string IS_Active { get; set; }
		public string User_Permissions_Type_Name_AR { get; set; }
		public string User_Permissions_Type_Name_EN { get; set; }
		public virtual JobDTO Job { get; set; }
	}
}
