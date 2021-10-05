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
	public class AppTypeController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public IHttpActionResult GetActive(int SID, int ReqType)
		{
			return Ok(new ResponseClass() { success = true, result = p.ValidTo.Where(q => q.Main_Services.IS_Action.Value && q.Main_Services.ServiceTypeID == SID && q.Applicant_Type.IS_Action.Value && q.Main_Services.UnitMainServices.Count(w => w.Units.Units_Request_Type.Count(s => s.Request_Type_ID == ReqType) != 0 && w.Units.IS_Action.Value) != 0).Select(q => q.Applicant_Type).Distinct() });
		}
	}
}
