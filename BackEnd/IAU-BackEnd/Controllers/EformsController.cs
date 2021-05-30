using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IAU_BackEnd.Controllers
{
	public class EformsController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		public IHttpActionResult GetActive(int SubService)
		{
			return Ok(new ResponseClass() { success = true, result = p.E_Forms.Where(q => q.IS_Action && q.SubServiceID == SubService).Select(q => new { Url = q.Path, q.Name_EN, Name_AR = q.Name }) });
		}
	}
}
