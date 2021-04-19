using IAU_BackEnd.Controllers.BasicData;
using IAU_BackEnd.Controllers.Request;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using IAU.DTO.Helper;
using System.Net.Http;
using System.Net;

namespace IAU_BackEnd.Controllers.Applicant
{
	public class ApplicantDataController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		 

		[HttpGet]
		[Route("api/ApplicantData/loadApplicantData")]
		public HttpResponseMessage LoadPersonalDataPageRequire()
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();
				var Countries = CountryController.GetCountriesList(Device_Info);

				var type = ApplicantController.GetTypesList(Device_Info);

				var titles =  ApplicantTitleController.GetApplicantMiddleTitlesList(Device_Info);

				var Cities = CountryController.GetCitiesList(Device_Info[2]);

				var Regions = CountryController.GetRegionsList(Device_Info[2]);

				var nationalty = NationaltyController.GetNationaltyList(Device_Info);
                var doctype = DocumentController.GetDocumentTypeList(Device_Info);

                return Request.CreateResponse(new ResponseClass
				{
					success = true,
					result = new
					{
						Countries,
						type,
						titles,
						nationalty,
						Cities,
						Regions,
						doctype
					}
				});
				 
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		[HttpPost]
		[Route("SaveApplicant_Data")]
		public async Task<IHttpActionResult> CreatePersonalData([FromBody] Personel_Data personel)
		{
			try
			{
				List<string> Device_Info = API_HelperFunctions.Get_DeviceInfo();

				var data = p.Personel_Data.Add(personel);
				if (p.SaveChanges() > 0)
					return Ok(new
					{
						success = true,
						result = data
					});
				return Ok(new
				{
					success = false
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


	}
}