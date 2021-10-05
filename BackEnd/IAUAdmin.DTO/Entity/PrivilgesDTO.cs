using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class PrivilgesDTO
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Name_EN { get; set; }
		public Nullable<int> SubOF { get; set; }
		public Nullable<DateTime> CreatedOn{ get; set; }
		public Nullable<bool> Active { get; set; }

		public virtual ICollection<PrivilgesDTO> Sub { get; set; }
		public virtual ICollection<PrivilgesDTO> Details { get; set; }
	}
}
