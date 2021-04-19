using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IAU.DTO.Helper;
using IAU.DTO.Entity;

namespace IAU_BackEnd.Controllers.BasicData

{
	public class NationaltyController : ApiController
	{
        private static MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		[Route("GetNationaltyNames")]
		public async Task<IHttpActionResult> GetNationalty()
        {
            try
            {
                List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();

                var data = GetNationaltyList(Device_Info);
                return Ok(new ResponseClass
                {
                    success = true,
                    result = data
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    success = false
                });
            }
        }
        public static IEnumerable<SelectList_DTO> GetNationaltyList(List<string> Device_Info)
        {
            try
            {
                string lang = Device_Info[2];
                var entity = p.Nationality.Where(a => a.IS_Action == true).ToList()
                  .Select(a =>
                new SelectList_DTO
                {
                    ID = a.Nationality_ID,
                    Name = (lang == "1" ? a.Nationality_Name_AR : a.Nationality_Name_EN),

                 });
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}