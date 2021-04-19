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
	public class ProviderAcademicServicesController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		[Route("GetAll")]
		public async Task<IHttpActionResult> GetAllProviderAcademicServices()
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
				var entity = GetProviderAcademicServicesList(Device_Info);
				return Ok(new ResponseClass
				{
					success = true,
					result = entity
				});
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					success = false
				});
			}
		}

		public  IEnumerable<SelectList_DTO> GetProviderAcademicServicesList(List<string> Device_Info)
		{
			try
			{
				string lang = Device_Info[2];
				var entity = p.Provider_Academic_Services.Where(a => a.IS_Action == true).ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.Provider_Academic_Services_ID,
					Name = (lang == "1" ? a.Provider_Academic_Services_Name_AR : a.Provider_Academic_Services_Name_EN),

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
