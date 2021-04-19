using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using Model.DTO;

namespace IAU_BackEnd.Controllers.BasicData
{
    public class RequestTypeController : ApiController
    {
        private MostafidDatabaseEntities p = new MostafidDatabaseEntities();

        [HttpGet]
        [Route("api/RequestType/GetAllRequestType")]
        public HttpResponseMessage GetAllRequestType()
        {
            try
            {
                List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
                string lang = Device_Info[2];
                var RequestType = p.Request_Type.Where(a => a.IS_Action == true).ToList()
                  .Select(a =>
                new SelectListItemDto
                {
                    Id = a.Request_Type_ID,
                    Name = (lang == "1" ? a.Request_Type_Name_AR : a.Request_Type_Name_EN),
                    ImagePath = a.Image_Path,
                });
                return Request.CreateResponse(new ResponseClass
                {
                    success = true,
                    result = new
                    {
                        RequestType,
                    }
                });
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}