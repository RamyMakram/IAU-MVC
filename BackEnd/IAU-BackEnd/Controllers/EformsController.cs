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
			return Ok(new ResponseClass() { success = true, result = p.Sub_Services.Where(q => q.IS_Action.Value && q.Sub_Services_ID == SubService) });
		}
	}
}
