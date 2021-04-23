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
	public class UnitsController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		// GET: api/Units
		public async Task<IHttpActionResult> GetUnits()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units });
		}

		public async Task<IHttpActionResult> GetUnits(int id)
		{
			var units = await p.Units.Where(q => q.Units_ID == id).Select(q => new { q.Units_ID, q.Units_Name_AR, q.Units_Name_EN, q.Units_Location_ID, q.Units_Type_ID, q.ServiceType_ID, q.Ref_Number, q.Building_Number, q.IS_Action, q.IS_Mostafid, q.Units_Type, q.Units_Location, Request_Type = q.Units_Request_Type.Select(w => new { w.Request_Type.Image_Path, w.Request_Type.Request_Type_Name_AR, w.Request_Type.Request_Type_Name_EN }), q.Service_Type, Units_Request_Type = q.Units_Request_Type.Select(s => new { s.Request_Type_ID, s.Units_ID, s.Units_Request_Type_ID }) }).FirstOrDefaultAsync();
			if (units == null)
				return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

			return Ok(new ResponseClass() { success = true, result = units });
		}
		[HttpGet]
		public async Task<IHttpActionResult> ThereIsNoMostafid()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units.Count(q => q.IS_Mostafid) == 0 });
		}
		public async Task<IHttpActionResult> Update(Units units)
		{
			var data = p.Units.Include(q => q.Units_Request_Type).FirstOrDefault(q => q.Units_ID == units.Units_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Units_Name_AR = units.Units_Name_AR;
				data.Units_Name_EN = units.Units_Name_EN;
				data.ServiceType_ID = units.ServiceType_ID;
				data.Units_Location_ID = units.Units_Location_ID;
				data.Units_Type_ID = units.Units_Type_ID;
				data.ServiceType_ID = units.ServiceType_ID;
				data.Building_Number = units.Building_Number;
				data.Ref_Number = units.Ref_Number;
				data.IS_Mostafid = units.IS_Mostafid;
				p.Units_Request_Type.RemoveRange(data.Units_Request_Type);
				data.Units_Request_Type = units.Units_Request_Type;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false });
			}
		}

		public async Task<IHttpActionResult> Create(Units units)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			units.IS_Action = true;
			p.Units.Add(units);
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}

		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Units units = await p.Units.FindAsync(id);
			if (units == null)
				return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

			units.IS_Action = true;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int id)
		{
			Units units = await p.Units.FindAsync(id);
			if (units == null)
				return Ok(new ResponseClass() { success = false, result = "Unit Is NULL" });

			units.IS_Action = false;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				p.Dispose();
			}
			base.Dispose(disposing);
		}
	}
}