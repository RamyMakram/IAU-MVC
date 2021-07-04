using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAUBackEnd.Admin.Controllers
{
	public class StatisticsController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();
		public async Task<IHttpActionResult> Get()
		{
			var MostafidCount = p.Service_Type.Select(q => new { AR = q.Service_Type_Name_AR, EN = q.Service_Type_Name_EN, IMG = q.Image_Path, Count = q.Request_Data.Count() }).ToList();
			var Stat = new { Units = p.Units.Count(), Requests = p.Request_Data.Count(), MainServices = p.Main_Services.Count(), SubServices = p.Sub_Services.Count() };
			var Locations = p.Location.Select(q => new { AR = q.Location_Name_AR, EN = q.Location_Name_EN, Count = (int?)q.Units_Location.Sum(s => (int?)s.Units.Sum(w => w.Request_Data.Count) ?? 0) ?? 0 }).OrderByDescending(q => q.Count).Take(4).ToList();
			return Ok(new ResponseClass() { success = true, result = new { MostafidCount, Stat, Locations } });
		}
	}
}