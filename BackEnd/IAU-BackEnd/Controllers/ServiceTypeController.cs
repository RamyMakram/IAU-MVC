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
    public class ServiceTypeController : ApiController
    {
        private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
        public async Task<ICollection<Service_Type>> GetActive()
        {
            //choose validto becuase must service attached to unit
            var data = p.ValidTo.Where(q =>
                !q.Deleted &&
                q.Main_Services.IS_Action.Value &&
                q.Main_Services.Service_Type.IS_Action.Value &&
                q.Applicant_Type.IS_Action.Value &&
                q.Main_Services.UnitMainServices.Count(s =>
                    s.Units.IS_Action.Value &&
                    !s.Units.Deleted &&
                    !s.Main_Services.Deleted &&
                    s.Main_Services.IS_Action.Value &&
                    s.Main_Services.Sub_Services.Count(t =>
                        t.IS_Action.Value &&
                        !t.Deleted
                        ) != 0
                    ) != 0
            ).Select(q => q.Main_Services.Service_Type).Distinct().ToList();
            return data;
        }
    }
}
