using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAU_BackEnd.Controllers
{
	public class RequestTypeController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public async Task<IHttpActionResult> GetActive(int SID)
		{
			return Ok(new ResponseClass() { success = true, result = p.Request_Type.Where(q => q.IS_Action.Value && q.Units_Request_Type.Count(w => (w.Units.ServiceTypeID == SID || w.Units.UnitServiceTypes.Count(s => s.ServiceTypeID == SID) != 0) && w.Units.UnitMainServices.Count(r => r.Main_Services.IS_Action.Value && r.Main_Services.Sub_Services.Count(g => g.IS_Action.Value) != 0) != 0) != 0).Select(q => new { ID = q.Request_Type_ID, Name_AR = q.Request_Type_Name_AR, Name_EN = q.Request_Type_Name_EN, q.Image_Path }) });
		}
	}
}
