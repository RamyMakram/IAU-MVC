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
        public IHttpActionResult GetActive(int UID, int ServiceID, int AppType)//Call if inquiry
        {
            var data = p.UnitMainServices.Where(q =>
                q.Units.IS_Action.Value &&
                q.Main_Services.IS_Action.Value &&
                q.Main_Services.ServiceTypeID == ServiceID &&
                q.UnitID == UID &&
                q.Main_Services.ValidTo.Any(w =>
                    w.ApplicantTypeID == AppType
                ) &&
                q.Main_Services.Sub_Services.Count(r =>
                    r.IS_Action.Value
                ) != 0
            ).Select(q => new { ID = q.Main_Services.Main_Services_ID, Name_AR = q.Main_Services.Main_Services_Name_AR, Name_EN = q.Main_Services.Main_Services_Name_EN });
            return Ok(new ResponseClass() { success = true, result = data });
        }
    }
}
