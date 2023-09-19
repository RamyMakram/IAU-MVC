using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [Route("api/_Home/LoadMain")]

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

        [HttpGet]
        [Route("api/_Home/LoadNewAndFlollowRequestLogin")]
        public async Task<IHttpActionResult> LoadNewAndFlollowRequestLogin()
        {
            try
            {
                var db = new MostafidDatabaseEntities();
                var New_Request_Login = await db.AppSetting.FirstOrDefaultAsync(q => q.Key == "NewAndFlollowRequestLogin");
                return Ok(new ResponseClass() { success = true, result = New_Request_Login?.Value });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
    }
}
