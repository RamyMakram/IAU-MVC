using IAU.DTO.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAU_BackEnd.Controllers
{
    public class _HomeController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> LoadMain()
        {
            try
            {
                var Service_Type = await new ServiceTypeController().GetActive();
                var Titles = await new TitlesController().GetActive();
                var Country = await new CountryController().GetActive();
                var Region = await new CountryController().GetActiveRegion();
                var City = await new CountryController().GetActiveCity();
                var IDS = await new IDDOcController().GetActive();
                return Ok(new ResponseClass() { success = true, result = new { Service_Type, Titles, Country, IDS, Region, City } });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
    }
}
