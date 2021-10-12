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
		public async Task<IHttpActionResult> FollowRequest([FromBody] string Code)
		{
			try
			{
				Code = Code.ToUpper();
				var data = p.Request_Data.Include(q => q.Units).Include(q => q.Request_State).Where(q => q.Code_Generate == Code)
					.Select(q => new { q.IsTwasul_OC, q.Request_State, q.Request_Data_ID, q.Request_State_ID }).FirstOrDefault();
				if (data == null)
					return Ok(new ResponseClass() { success = false, result = null });

				return Ok(new ResponseClass() { success = true, result = new { Request = data, State = p.RequestTransaction.Where(q => q.Request_ID == data.Request_Data_ID).Include(q => q.Units).OrderByDescending(w => w.ID).FirstOrDefault() } });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
		[HttpGet]
		[Route("api/Request/SendSMS")]
		public async Task<IHttpActionResult> SendSMS(string Mobile, string message)
		{
			try
			{
				HttpClient h = new HttpClient();

				string url = $"http://basic.unifonic.com/wrapper/sendSMS.php?appsid=f9iRotRBsanfAB0xcE4NzJtgMYf5Bk&msg={message}&to={Mobile}&sender=IAU-BSC&baseEncode=False&encoding=UCS2";
				h.BaseAddress = new Uri(url);

				var res = h.GetAsync("").Result.Content.ReadAsStringAsync().Result;
				return Ok(new ResponseClass()
				{
					result = res,
					success = true
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass()
				{
					result = ee,
					success = false
				});
			}
		}

	}
}
