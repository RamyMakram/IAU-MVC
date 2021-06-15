using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAUAdmin.DTO.Entity
{
	public class Request_TransactionDTO
	{
		public int ID { get; set; }
		public int Request_ID { get; set; }
		public int FromUnitID { get; set; }
		public Nullable<int> ToUnitID { get; set; }
		public Nullable<System.DateTime> ForwardDate { get; set; }
		public Nullable<System.DateTime> ExpireDays { get; set; }
		public string Comment { get; set; }
		public int? CommentType { get; set; }
		public bool Readed { get; set; }
		public string MostafidComment { get; set; }
		public string Code { get; set; }


		public Nullable<System.DateTime> ReadedDate { get; set; }
		public virtual UnitsDTO Units1 { get; set; }
		public Nullable<System.DateTime> CommentDate { get; set; }

		public virtual ReqestDTO Request_Data { get; set; }
	}
}
