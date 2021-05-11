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
	public class UnitLevelsController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetUnitLevel()
		{
			return Ok(new ResponseClass() { success = true, result = p.UnitLevel });
		}

		public async Task<IHttpActionResult> GetUnitLevel(int id)
		{
			UnitLevel unitLevel = await p.UnitLevel.FindAsync(id);
			if (unitLevel == null)
				return Ok(new ResponseClass() { success = false, result = "Level Is Null" });

			return Ok(new ResponseClass() { success = true, result = unitLevel });
		}

		public async Task<IHttpActionResult> Update(UnitLevel unitLevel)
		{
			var data = p.UnitLevel.FirstOrDefault(q => q.ID == unitLevel.ID);
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Name_AR = unitLevel.Name_AR;
				data.Name_EN = unitLevel.Name_EN;
				data.Code = unitLevel.Code;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });

			}
		}

		public async Task<IHttpActionResult> Create(UnitLevel unitLevel)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });

			p.UnitLevel.Add(unitLevel);
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