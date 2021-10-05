using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
namespace IAU_BackEnd.Controllers
{
	public class RequestController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		[HttpPost]
		public async Task<IHttpActionResult> FollowRequest([FromBody]string Code)
		{
			try
			{
				var data = p.Request_Data.Include(q => q.Units).Include(q => q.Request_State).Where(q => q.Code_Generate == Code)
					.Select(q => new { q.IsTwasul_OC, q.Request_State, q.Request_Data_ID, q.Request_State_ID }).FirstOrDefault();
				return Ok(new ResponseClass() { success = true, result = new { Request = data, State = p.RequestTransaction.Where(q => q.Request_ID == data.Request_Data_ID).Include(q => q.Units).OrderByDescending(w => w.ID).FirstOrDefault() } });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
	}
}
