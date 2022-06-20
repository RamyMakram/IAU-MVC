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
            var data = p.Request_Type.Where(q =>
                !q.Deleted &&
                q.IS_Action.Value &&
                q.Units_Request_Type.Count(w =>
                    !w.Units.Deleted &&
                    (//check if unit service type "main and sub" contain service type
                        w.Units.ServiceTypeID == SID ||
                        w.Units.UnitServiceTypes.Any(s =>
                            s.ServiceTypeID == SID
                        )
                    )
                    && w.Units.UnitMainServices.Count(r =>//check unit main service 
                        !r.Main_Services.Deleted &&
                        r.Main_Services.IS_Action.Value &&
                        r.Main_Services.Sub_Services.Count(g =>
                            g.IS_Action.Value &&
                            !g.Deleted
                            ) != 0
                        ) != 0

                ) != 0
            ).Select(q => new { ID = q.Request_Type_ID, N_AR = q.Request_Type_Name_AR, N_EN = q.Request_Type_Name_EN, q.Image_Path, DAR = q.Desc_AR, DEN = q.Desc_EN });
            return Ok(new ResponseClass() { success = true, result = data });
        }
    }
}
