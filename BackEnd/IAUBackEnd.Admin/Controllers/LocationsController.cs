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
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
	public class LocationsController : ApiController
	{
		private MostafidDBEntities db = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetLocations()
		{
			return Ok(new ResponseClass() { success = true, result = db.Location });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = db.Location.Where(q => q.IS_Action.Value) });
		}
		public async Task<IHttpActionResult> GetLocation(int id)
		{
			Location location = await db.Location.FindAsync(id);
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
				var data = db.Location.FirstOrDefault(q => q.Location_ID == location.Location_ID);
				if (!ModelState.IsValid || data == null)
					return Ok(new ResponseClass() { success = false, result = ModelState });
				var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

				var trans = db.Database.BeginTransaction();

				data.Location_Name_AR = location.Location_Name_AR;
				data.Location_Name_EN = location.Location_Name_EN;
				await db.SaveChangesAsync();
				var logstate = Logger.AddLog(db: db, logClass: LogClassType.Locations, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Location_ID, notes: "");
				if (logstate)
				{
					await db.SaveChangesAsync();
					trans.Commit();
					return Ok(new ResponseClass()
					{
						success = true
					});
				}
				else
				{
					trans.Rollback();
					return Ok(new ResponseClass() { success = false });
				}
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
			var trans = db.Database.BeginTransaction();

			location.IS_Action = true;
			db.Location.Add(location);
			await db.SaveChangesAsync();

			var logstate = Logger.AddLog(db: db, logClass: LogClassType.Locations, Method: "Update", Oldval: null, Newval: location, es: out _, syslog: out _, ID: location.Location_ID, notes: "");
			if (logstate)
			{
				await db.SaveChangesAsync();
				trans.Commit();
				return Ok(new ResponseClass()
				{
					success = true
				});
			}
			else
			{
				trans.Rollback();
				return Ok(new ResponseClass() { success = false });
			}
		}
		[HttpGet]
		public async Task<IHttpActionResult> DeactiveLocation(int id)
		{
			Location location = await db.Location.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}
			var trans = db.Database.BeginTransaction();

			location.IS_Action = false;

			await db.SaveChangesAsync();


			var logstate = Logger.AddLog(db: db, logClass: LogClassType.Locations, Method: "Deactive", Oldval: null, Newval: null, es: out _, syslog: out _, ID: location.Location_ID, notes: "");
			if (logstate)
			{
				await db.SaveChangesAsync();
				trans.Commit();
				return Ok(new ResponseClass()
				{
					success = true
				});
			}
			else
			{
				trans.Rollback();
				return Ok(new ResponseClass() { success = false });
			}
		}
		[HttpGet]
		public async Task<IHttpActionResult> ActiveLocation(int id)
		{
			Location location = await db.Location.FindAsync(id);
			if (location == null)
			{
				return NotFound();
			}
			var trans = db.Database.BeginTransaction();

			location.IS_Action = true;

			await db.SaveChangesAsync();


			var logstate = Logger.AddLog(db: db, logClass: LogClassType.Locations, Method: "Active", Oldval: null, Newval: null, es: out _, syslog: out _, ID: location.Location_ID, notes: "");
			if (logstate)
			{
				await db.SaveChangesAsync();
				trans.Commit();
				return Ok(new ResponseClass()
				{
					success = true
				});
			}
			else
			{
				trans.Rollback();
				return Ok(new ResponseClass() { success = false });
			}
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