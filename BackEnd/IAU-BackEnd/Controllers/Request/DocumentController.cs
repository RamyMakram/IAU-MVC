using IAU_BackEnd.Controllers.BasicData;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using IAU.DTO.Helper;
using IAU.DTO.Entity;

namespace IAU_BackEnd.Controllers.Request
{
	public class DocumentController : ApiController
	{
		private static MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		//[Route("api/Document/GetDocument/{Main_Service_ID}/{Sub_Service_ID}")]
		//public async Task<IHttpActionResult> GetDocument_By_MainAndSub(int Main_Service_ID, int Sub_Service_ID)
		//{
		//	try
		//	{
		//		var data = p.Documents.Where(q => q.Main_Services_ID == Main_Service_ID && q.Sub_Services_ID == Sub_Service_ID);
		//		return Ok(new
		//		{
		//			success = true,
		//			result = data
		//		});
		//	}
		//	catch (Exception e)
		//	{
		//		return Ok(new
		//		{
		//			success = false
		//		});
		//	}
		//}

		[HttpGet]
		//[Route("GetSupportedDocument")]
		public async Task<IHttpActionResult> GetSupportedDocument(string lang)
		{
			try
			{
				var data = GetSupportedDocumentList(lang);
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

		[HttpGet]
		[Route("api/Document/loadpage")]
		public HttpResponseMessage LoadDocumentPageRequire(string lang)
		{
			try
			{
				var provider = (new ProviderAcademicServicesController().GetProviderAcademicServicesList(lang));

				var supporteddocs = GetSupportedDocumentList(lang);

				return Request.CreateResponse(new ResponseClass
				{
					success = true,
					result = new
					{
						provider,
						//mainServices,
						
						supporteddocs
					}
				});
			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
			}
		}

		public static IEnumerable<SelectList_DTO> GetSupportedDocumentList(string lang)
		{
			try
			{
				var entity = p.Supporting_Documents.Where(a => a.IS_Action == true).ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.Supporting_Documents_ID,
					Name = (lang == "ar" ? a.Supporting_Documents_Name_AR : a.Supporting_Documents_Name_EN),

				});
				return entity;
			}
			catch (Exception ex)
			{
				return null;
			}
		}


		public static IEnumerable<SelectList_DTO> GetDocumentTypeList(List<string> Device_Info)
		{
			try
			{
				string lang = Device_Info[2];
				var entity = p.ID_Document.ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.ID_Document1,
					Name = (lang == "ar" ? a.Document_Name_AR : a.Document_Name_EN),

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
