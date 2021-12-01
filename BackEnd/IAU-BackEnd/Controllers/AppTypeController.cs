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
            var req = p.Request_Type.Any(q => q.Request_Type_ID == ReqType && q.Request_Type_Name_EN.ToLower().Contains("inq"));
            var data = p.ValidTo.Where(q =>
            q.Main_Services.IS_Action.Value &&
            q.Main_Services.ServiceTypeID == SID &&//get main service
            q.Applicant_Type.IS_Action.Value &&
            (req ? true : q.Main_Services.Sub_Services.Any(g => g.IS_Action.Value)) &&//check inquiry for sub service
            q.Main_Services.UnitMainServices.Count(w =>//check if main service exist in unit main service
                w.Units.IS_Action.Value &&//check if unit is active
                w.Units.Units_Request_Type.Count(s =>//check request type
                    s.Request_Type_ID == ReqType
                    ) != 0
                ) != 0
            ).Select(q => q.Applicant_Type).Distinct().Select(q => new { q.Applicant_Type_Name_AR, q.Applicant_Type_Name_EN, q.Affliated, q.Applicant_Type_ID });
            var ss = data.ToList();
            return Ok(new ResponseClass() { success = true, result = data });
        }
    }
}
