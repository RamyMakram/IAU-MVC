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
	public class Sub_ServicesController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetSub_Services()
		{
			return Ok(new ResponseClass() { success = true, result = p.Sub_Services });
		}
		public async Task<IHttpActionResult> GetSub_ServicesByMain(int id)
		{
			return Ok(new ResponseClass() { success = true, result = p.Sub_Services.Where(q => q.Main_Services_ID == id) });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Sub_Services.Where(q => q.IS_Action.Value) });
		}

		public async Task<IHttpActionResult> GetSub_Services(int id)
		{
			Sub_Services sub_Services = p.Sub_Services.Include(q => q.Required_Documents).Include(q => q.Main_Services).Include(q => q.Main_Services.Service_Type).FirstOrDefault(q => q.Sub_Services_ID == id);
			if (sub_Services == null)
				return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });

			return Ok(new ResponseClass() { success = true, result = sub_Services });
		}

		public async Task<IHttpActionResult> Update(Sub_Services sub_Services)
		{
			var data = p.Sub_Services.Include(q => q.Required_Documents).FirstOrDefault(q => q.Sub_Services_ID == sub_Services.Sub_Services_ID);
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Sub_Services_Name_AR = sub_Services.Sub_Services_Name_AR;
				data.Sub_Services_Name_EN = sub_Services.Sub_Services_Name_EN;
				data.Main_Services_ID = sub_Services.Main_Services_ID;
				foreach (var i in sub_Services.Required_Documents)
				{
					if (i.ID == null)
						data.Required_Documents.Add(i);
					else
					{
						var ss = data.Required_Documents.FirstOrDefault(q => q.ID == i.ID);
						ss.Name_AR = i.Name_AR;
						ss.Name_EN = i.Name_EN;
						ss.IS_Action = i.IS_Action;
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

		public async Task<IHttpActionResult> Create(Sub_Services sub_Services)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				sub_Services.IS_Action = true;
				p.Sub_Services.Add(sub_Services);
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Sub_Services sub_Services = await p.Sub_Services.FindAsync(id);
			if (sub_Services == null)
				return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });

			sub_Services.IS_Action = true;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int id)
		{
			Sub_Services sub_Services = await p.Sub_Services.FindAsync(id);
			if (sub_Services == null)
				return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });

			sub_Services.IS_Action = false;
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