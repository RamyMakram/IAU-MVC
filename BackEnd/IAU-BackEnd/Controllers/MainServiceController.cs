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
	public class MainServiceController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public IHttpActionResult GetActive(int UID, int ServiceID)
		{
			return Ok(new ResponseClass() { success = true, result = p.UnitMainServices.Where(q => q.Units.IS_Action.Value && q.Main_Services.ServiceTypeID == ServiceID && q.UnitID == UID).Select(q => q.Main_Services) });
		}
	}
}
