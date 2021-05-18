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
		public async Task<IHttpActionResult> GetUnitLevelForUnit()
		{
			return Ok(new ResponseClass() { success = true, result = p.UnitLevel.Where(q => q.Units_Type.Count() > 0) });
		}

		public async Task<IHttpActionResult> GetUnitLevel(int id)
		{
			UnitLevel unitLevel = await p.UnitLevel.Include(q => q.Units_Type).FirstOrDefaultAsync(q => q.ID == id);
			if (unitLevel == null)
				return Ok(new ResponseClass() { success = false, result = "Level Is Null" });

			return Ok(new ResponseClass() { success = true, result = unitLevel });
		}

		public async Task<IHttpActionResult> Update(UnitLevel unitLevel)
		{
			var data = p.UnitLevel.Include(q => q.Units_Type).FirstOrDefault(q => q.ID == unitLevel.ID);
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Name_AR = unitLevel.Name_AR;
				data.Name_EN = unitLevel.Name_EN;
				data.Code = unitLevel.Code;
				foreach (var i in unitLevel.Units_Type)
				{
					if (i.Units_Type_ID == null || i.Units_Type_ID == 0)
						data.Units_Type.Add(i);
					else
					{
						var ss = data.Units_Type.FirstOrDefault(q => q.Units_Type_ID == i.Units_Type_ID);
						ss.Units_Type_Name_AR = i.Units_Type_Name_AR;
						ss.Units_Type_Name_EN = i.Units_Type_Name_EN;
						ss.IS_Action = i.IS_Action;
						ss.Code = i.Code;
					}
				}

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