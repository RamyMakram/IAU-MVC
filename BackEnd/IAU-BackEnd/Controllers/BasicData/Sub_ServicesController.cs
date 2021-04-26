using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IAU.DTO.Entity;
using IAU.DTO.Helper;
using IAU_BackEnd.Models;

namespace IAU_BackEnd.Controllers.BasicData

{
    public class Sub_ServicesController : ApiController
    {
        private MostafidDatabaseEntities p = new MostafidDatabaseEntities();

        [HttpGet]
        [Route("api/Sub_Services/GetSubServices")]
        public HttpResponseMessage GetSubServices(int main_ID )
        {
            try
            {
                List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
                string lang = Device_Info[2];
                var subServices = p.Sub_Services.Where(a =>a.IS_Action==true && a.Main_Services_ID == main_ID)
                  .Select(a =>
                new SelectList_DTO
                {
                    ID = a.Sub_Services_ID,
                    Name = (lang == "ar" ? a.Sub_Services_Name_AR : a.Sub_Services_Name_EN),

                }).ToList();
                return Request.CreateResponse(System.Net.HttpStatusCode.OK,
                    new ResponseClass
                    {
                        success = true,
                        result = new { subServices }
                    });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}