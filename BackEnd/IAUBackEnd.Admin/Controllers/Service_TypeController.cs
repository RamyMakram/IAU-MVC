using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
	public class Service_TypeController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetService_Type()
		{
			return Ok(new ResponseClass() { success = true, result = p.Service_Type });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Service_Type.Where(q => q.IS_Action.Value) });
		}

		public async Task<IHttpActionResult> GetService_Type(int id)
		{
			Service_Type service_Type = await p.Service_Type.FindAsync(id);
			if (service_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

			return Ok(new ResponseClass() { success = true, result = service_Type });
		}

		public async Task<IHttpActionResult> Update(Service_Type service_Type)
		{
			var data = p.Service_Type.FirstOrDefault(q => q.Service_Type_ID == service_Type.Service_Type_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Service_Type_Name_AR = service_Type.Service_Type_Name_AR;
				data.Service_Type_Name_EN = service_Type.Service_Type_Name_EN;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> Create(ServiceTypeDTO service_Type)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });

			try
			{
				var path = HttpContext.Current.Server.MapPath("~");
				var FilePath = Path.Combine("Images", "Service_Types", DateTime.Now.Ticks.ToString() + ".png");
				p.Service_Type.Add(new Service_Type() { IS_Action = true, Service_Type_Name_AR = service_Type.Service_Type_Name_AR, Service_Type_Name_EN = service_Type.Service_Type_Name_EN, Image_Path = FilePath.Replace('\\', '/') });
				File.WriteAllBytes(Path.Combine(path, FilePath), Convert.FromBase64String(service_Type.Base64));
				await p.SaveChangesAsync();
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
			Service_Type service_Type = await p.Service_Type.FindAsync(id);
			if (service_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

			service_Type.IS_Action = false;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Service_Type service_Type = await p.Service_Type.FindAsync(id);
			if (service_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

			service_Type.IS_Action = true;
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