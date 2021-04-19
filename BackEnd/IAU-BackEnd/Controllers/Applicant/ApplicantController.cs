using IAU.DTO.Entity;
using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAU_BackEnd.Controllers.Applicant
{
	public class ApplicantController : ApiController
	{
		private  static MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		[Route("GetApplicantNames")]
		public async Task<IHttpActionResult> GetTypes()
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();

				var entity = GetTypesList(Device_Info);
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
		public  static IEnumerable<SelectList_DTO> GetTypesList(List<string> Device_Info)
		{
			try
			{
				string lang = Device_Info[2];
				var entity = p.Applicant_Type.Where(a=>a.IS_Action==true).ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.Applicant_Type_ID,
					Name = (lang == "1" ? a.Applicant_Type_Name_AR : a.Applicant_Type_Name_EN),
				 
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
