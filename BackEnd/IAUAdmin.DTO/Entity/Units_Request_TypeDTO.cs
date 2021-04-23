using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class Units_Request_TypeDTO
	{
		public int Units_Request_Type_ID { get; set; }
		public Nullable<int> Units_ID { get; set; }
		public Nullable<int> Request_Type_ID { get; set; }
	}
}
