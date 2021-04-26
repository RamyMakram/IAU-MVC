using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using AutoMapper;
using IAU.DTO.Entity;
using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using Newtonsoft.Json;

namespace IAU_BackEnd.Controllers.BasicData

{
	[EnableCors(origins: "*", headers: "*", methods: "*")]
	public class Main_ServiceController : ApiController
	{
		private static MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		[HttpGet]
		[Route("api/Main_Service/GetMainServices")]
		public HttpResponseMessage GetMainServices(int provideId = 0)
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
				var mainServices = GetMainServicesList(provideId, Device_Info);
				return Request.CreateResponse(new ResponseClass
				{
					success = true,
					result = new
					{
						mainServices,
					}
				});
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, JsonConvert.SerializeObject(ex));
			}
		}
		public static IEnumerable<SelectList_DTO> GetMainServicesList(int provideId, List<string> Device_Info)
		{
			string lang = Device_Info[2];
			var entity = p.Main_Services.Where(a => a.IS_Action == true)
			  .Select(a =>
			new SelectList_DTO
			{
				ID = a.Main_Services_ID,
				Name = (lang == "ar" ? a.Main_Services_Name_AR : a.Main_Services_Name_EN),
			}).ToList();
			return entity;

		}
	}
}