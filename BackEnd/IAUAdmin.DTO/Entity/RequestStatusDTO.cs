using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class RequestStatusDTO
	{
		public byte State_ID { get; set; }
		public string StateName_AR { get; set; }
		public string StateName_EN { get; set; }
	}
}
