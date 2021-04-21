using System;

namespace IAU.DTO.Entity
{
	public class FollowRequest_DTO
	{
		public byte statusId;

		public string requestCode { get; set; }
		public int requestid { get; set; } = 0;
		public string location { get; set; }
		public string status { get; set; }
		public DateTime deliverydate { get; set; }
	}
}
