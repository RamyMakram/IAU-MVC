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
	public class LocationsController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetLocations()
		{
			return Ok(new ResponseClass() { success = true, result = p.Location });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Location.Where(q => q.IS_Action.Value) });
		}
		public async Task<IHttpActionResult> GetLocation(int id)
		{
			Location location = await p.Location.FindAsync(id);
			if (location == null)
				return Ok(new ResponseClass() { success = false, result = "Location Is Null" });

			return Ok(new ResponseClass() { success = true, result = location });
		}

		// PUT: api/Locations/5
		[ResponseType(typeof(void))]
		public async Task<IHttpActionResult> UpdateLocation(Location location)
		{
			try
			{
				var data = p.Location.FirstOrDefault(q => q.Location_ID == location.Location_ID);
				if (!ModelState.IsValid || data == null)
					return Ok(new ResponseClass() { success = false, result = ModelState });
				data.Location_Name_AR = location.Location_Name_AR;
				data.Location_Name_EN = location.Location_Name_EN;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ModelState });
			}
		}

		public async Task<IHttpActionResult> Create(Location location)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false });
			location.IS_Action = true;
			p.Location.Add(location);
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> DeactiveLocation(int id)
		{
			Location location = await p.Location.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}
			location.IS_Action = false;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> ActiveLocation(int id)
		{
			Location location = await p.Location.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}
			location.IS_Action = true;
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