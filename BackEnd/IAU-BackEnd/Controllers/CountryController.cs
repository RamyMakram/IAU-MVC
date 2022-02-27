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
    public class CountryController : ApiController
    {
        private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
        [NonAction]
        public async Task</*ICollection<IAU.DTO.Entity.CountryDTO>*/object> GetActive()
        {
            return p.Country.Where(q => q.IS_Action.Value).OrderBy(q => q.Index).Select(q => new { q.Country_ID, q.Country_Name_EN, q.Country_Name_AR, Regions = q.Region.Select(d => new { d.Region_Name_AR, d.Region_Name_EN, d.Region_ID, Cities = d.City }) }).ToList();
        }
        [NonAction]
        public async Task</*ICollection<IAU.DTO.Entity.CountryDTO>*/object> GetActiveRegion()
        {
            return p.Region;
        }
        [NonAction]
        public async Task</*ICollection<IAU.DTO.Entity.CountryDTO>*/object> GetActiveCity()
        {
            return p.City;
        }
    }
}
