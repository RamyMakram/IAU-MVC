using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
	public class GeneralSettingController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetStatusDelayeValue()
		{
			var data = p.Request_State.Select(s => new { s.State_ID, s.StateName_AR, s.StateName_EN, s.AllowedDelay });
			return Ok(new ResponseClass() { success = true, result = data });
		}

		[HttpPost]
		public async Task<IHttpActionResult> SaveDelayeValue([FromBody] List<Request_State> request_States)
		{
			foreach (var i in request_States)
			{
				var data = p.Request_State.FirstOrDefault(q => q.State_ID == i.State_ID);
				data.AllowedDelay = i.AllowedDelay;
			}
			await p.SaveChangesAsync();
			return Ok(new ResponseClass() { success = true });
		}
	}
}
