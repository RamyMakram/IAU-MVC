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
	public class Main_ServicesController : ApiController
	{
		private MostafidDBEntities db = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetMain_Services()
		{
			return Ok(new ResponseClass() { success = true, result = db.Main_Services });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = db.Main_Services.Where(q => q.IS_Action.Value) });
		}

		public async Task<IHttpActionResult> GetMain_Services(int id)
		{
			var main_Services = db.Main_Services.Include(q => q.ValidTo).Include(q => q.Units).Where(q => q.Main_Services_ID == id).Select(q => new { q.Main_Services_ID, q.Main_Services_Name_AR, q.Main_Services_Name_EN, q.IS_Action, q.Units, q.ValidTo, MainService_ApplicantType = q.ValidTo.Select(w => new { w.Applicant_Type.Applicant_Type_Name_AR, w.Applicant_Type.Applicant_Type_Name_EN }) }).FirstOrDefault();
			if (main_Services == null)
				return Ok(new ResponseClass() { success = false, result = "Main Is Null" });

			return Ok(new ResponseClass() { success = true, result = main_Services });
		}

		public async Task<IHttpActionResult> Update(Main_Services main_Services)
		{
			var data = db.Main_Services.Include(q => q.ValidTo).FirstOrDefault(q => q.Main_Services_ID == main_Services.Main_Services_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Main_Services_Name_AR = main_Services.Main_Services_Name_AR;
				data.Main_Services_Name_EN = main_Services.Main_Services_Name_EN;
				data.UnitID = main_Services.UnitID;
				db.ValidTo.RemoveRange(data.ValidTo);
				data.ValidTo = main_Services.ValidTo;
				data.IS_Action = true;
				await db.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> Create(Main_Services main_Services)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				db.Main_Services.Add(main_Services);
				await db.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int id)
		{
			Main_Services main_Services = await db.Main_Services.FindAsync(id);
			if (main_Services == null)
				return Ok(new ResponseClass() { success = false, result = "Main Is Null" });

			main_Services.IS_Action = false;
			await db.SaveChangesAsync();
			return Ok(new ResponseClass() { success = true });
		}

		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Main_Services main_Services = await db.Main_Services.FindAsync(id);
			if (main_Services == null)
				return Ok(new ResponseClass() { success = false, result = "Main Is Null" });

			main_Services.IS_Action = true;
			await db.SaveChangesAsync();
			return Ok(new ResponseClass() { success = true });
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