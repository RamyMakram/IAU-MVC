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
	public class UnitsController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public IHttpActionResult GetActive(int ReqID, int SerID)
		{
			return Ok(new ResponseClass() { success = true, result = p.Units.Where(q => q.IS_Action.Value && (q.ServiceTypeID == SerID || q.UnitServiceTypes.Count(d => d.ServiceTypeID == SerID) != 0) && q.Units_Request_Type.Count(w => w.Request_Type_ID == ReqID) != 0) });
		}
	}
}
