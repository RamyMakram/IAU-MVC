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
			var req = p.Request_Type.Any(q => q.Request_Type_ID == ReqID && q.Request_Type_Name_EN.ToLower().Contains("inq"));
			var data = p.Units.Where(q =>
			q.IS_Action.Value &&
			q.UnitMainServices.Count(w =>
				w.Main_Services.IS_Action.Value &&
				(req ? true : w.Main_Services.Sub_Services.Count(t => t.IS_Action.Value) != 0) &&//validated if inquiry then it's not required sub service
				w.Main_Services.ValidTo.Count(e => e.ApplicantTypeID == AppType) != 0)//Check Applicant Type in main service
			!= 0 &&
			(q.ServiceTypeID == SerID || q.UnitServiceTypes.Count(d => d.ServiceTypeID == SerID) != 0) && //check Service Type
			q.Units_Request_Type.Any(w => w.Request_Type_ID == ReqID)//check request type
			)
				.Select(q => new { ID = q.Units_ID, Name_AR = q.Units_Name_AR, Name_EN = q.Units_Name_EN }).ToList();
			return Ok(new ResponseClass() { success = true, result = data });
		}
		public IHttpActionResult GetActiveTest()
		{
			return Ok(new ResponseClass() { success = true, result = Request.Headers.Referrer });
		}
	}
}
