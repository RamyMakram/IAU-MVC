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
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
	public class Applicant_TypeController : ApiController
	{
		private MostafidDBEntities db = new MostafidDBEntities();

		// GET: api/Applicant_Type
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = db.Applicant_Type.Where(q => q.IS_Action == true) });
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}