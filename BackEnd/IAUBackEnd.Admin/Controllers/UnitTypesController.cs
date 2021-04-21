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
	public class UnitTypesController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetUnits_Type()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units_Type });
		}

		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Units_Type.Where(q => q.IS_Action.Value) });
		}

		public async Task<IHttpActionResult> GetUnits_Type(int id)
		{
			Units_Type units_Type = await p.Units_Type.FindAsync(id);
			if (units_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });

			return Ok(new ResponseClass() { success = true, result = units_Type });
		}

		public async Task<IHttpActionResult> Edit(Units_Type units_Type)
		{
			var data = p.Units_Type.FirstOrDefault(q => q.Units_Type_ID == units_Type.Units_Type_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Units_Type_Name_AR = units_Type.Units_Type_Name_AR;
				data.Units_Type_Name_EN = units_Type.Units_Type_Name_EN;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> Create(Units_Type units_Type)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });

			units_Type.IS_Action = true;
			p.Units_Type.Add(units_Type);
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}

		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int id)
		{
			Units_Type units_Type = await p.Units_Type.FindAsync(id);
			if (units_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });

			units_Type.IS_Action = false;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Units_Type units_Type = await p.Units_Type.FindAsync(id);
			if (units_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });

			units_Type.IS_Action = true;
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