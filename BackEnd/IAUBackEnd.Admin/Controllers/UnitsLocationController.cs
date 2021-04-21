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
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
	public class UnitsLocationController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetUnits_Location()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units_Location });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units_Location.Where(q => q.IS_Action.Value) });
		}

		public async Task<IHttpActionResult> GetUnits_Location(int id)
		{
			Units_Location units_Location = await p.Units_Location.FindAsync(id);
			if (units_Location == null)
				return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });

			return Ok(new ResponseClass() { success = true, result = units_Location });
		}
		public async Task<IHttpActionResult> GetLocationWithLang(int id, string lang)
		{
			var location = await p.Units_Location.FindAsync(id);
			if (location == null)
				return Ok(new ResponseClass() { success = false, result = "Location Is Null" });

			return Ok(new ResponseClass() { success = true, result = new UnitsLocDTO { Units_Location_ID = location.Units_Location_ID, IS_Action = location.IS_Action, Location_ID = location.Location_ID, Units_Location_Name_AR = location.Units_Location_Name_AR, Units_Location_Name_EN = location.Units_Location_Name_EN, Location = new LocationsDTO() { Location_ID = location.Location.Location_ID, Name = lang == "ar" ? location.Location.Location_Name_AR : location.Location.Location_Name_EN } } });
		}
		public async Task<IHttpActionResult> EditUnits_Location(Units_Location units_Location)
		{
			var data = p.Units_Location.FirstOrDefault(q => q.Units_Location_ID == units_Location.Units_Location_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Units_Location_Name_AR = units_Location.Units_Location_Name_AR;
				data.Units_Location_Name_EN = units_Location.Units_Location_Name_EN;
				data.Location_ID = units_Location.Location_ID;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> Create(Units_Location units_Location)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			units_Location.IS_Action = true;
			p.Units_Location.Add(units_Location);
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int id)
		{
			Units_Location units_Location = await p.Units_Location.FindAsync(id);
			if (units_Location == null)
				return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
			units_Location.IS_Action = false;
			await p.SaveChangesAsync();
			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Units_Location units_Location = await p.Units_Location.FindAsync(id);
			if (units_Location == null)
				return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
			units_Location.IS_Action = true;
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