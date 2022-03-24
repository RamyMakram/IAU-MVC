using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IAU_BackEnd.Controllers
{
	public class SubServicesController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public IHttpActionResult GetActive(int MainService)
		{
			var res = p.Sub_Services.Include(q => q.Required_Documents).Where(q => q.IS_Action.Value && q.Main_Services_ID == MainService).Select(q => new { ID = q.Sub_Services_ID, Name_AR = q.Sub_Services_Name_AR, Name_EN = q.Sub_Services_Name_EN, Docs = q.Required_Documents.Select(s => new { s.Name_AR, s.Name_EN, s.ID }) }).ToList();
			return Ok(new ResponseClass() { success = true, result = res });
		}
	}
}
