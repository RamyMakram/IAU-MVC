using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class DelayedTransDTO
	{

		public int ID { get; set; }
		public int RequestID { get; set; }
		public string RequestCode { get; set; }
		public byte RequestStatus { get; set; }
		public System.DateTime TransactionDate { get; set; }
		public int Delayed { get; set; }
		public System.DateTime AddedDate { get; set; }
		public bool Readed { get; set; }
		public virtual ReqestDTO Request_Data { get; set; }
		public virtual RequestStatusDTO Request_State { get; set; }

	}
}
