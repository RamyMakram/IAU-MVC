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
		public IHttpActionResult GetActive(int ReqID, int SerID, int AppType)
		{
			var data = p.Units.Where(q => q.IS_Action.Value && q.UnitMainServices.Count(w => w.Main_Services.IS_Action.Value && w.Main_Services.Sub_Services.Count(t => t.IS_Action.Value) != 0 && w.Main_Services.ValidTo.Count(e => e.ApplicantTypeID == AppType) != 0) != 0 && (q.ServiceTypeID == SerID || q.UnitServiceTypes.Count(d => d.ServiceTypeID == SerID) != 0) && q.Units_Request_Type.Count(w => w.Request_Type_ID == ReqID) != 0).Select(q => new { ID = q.Units_ID, Name_AR = q.Units_Name_AR, Name_EN = q.Units_Name_EN }).ToList();
			return Ok(new ResponseClass() { success = true, result = data });
		}
	}
}
