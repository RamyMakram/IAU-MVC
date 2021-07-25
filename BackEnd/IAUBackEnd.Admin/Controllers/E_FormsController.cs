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
	public class E_FormsController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetE_Forms()
		{
			return Ok(new ResponseClass() { success = true, result = p.E_Forms });
		}

		public async Task<IHttpActionResult> GetE_Forms(int id)
		{
			var e_Forms = p.E_Forms.Include(q => q.Sub_Services).FirstOrDefault(q => q.ID == id);
			if (e_Forms == null)
				return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });

			return Ok(new ResponseClass() { success = true, result = e_Forms });
		}
		public async Task<IHttpActionResult> GetE_FormsWithSubService(int id)
		{
			var e_Forms = p.E_Forms.Where(q => q.SubServiceID == id);
			if (e_Forms == null)
				return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });

			return Ok(new ResponseClass() { success = true, result = e_Forms });
		}

		public async Task<IHttpActionResult> Update(E_Forms e_Forms)
		{
			var data = p.E_Forms.FirstOrDefault(q => q.ID == e_Forms.ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Name = e_Forms.Name;
				data.Name_EN = e_Forms.Name_EN;
				data.SubServiceID = e_Forms.SubServiceID;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (DbUpdateConcurrencyException)
			{
				return Ok(new ResponseClass() { success = false });
			}
		}

		public async Task<IHttpActionResult> Create(E_FormsDTO e_Forms)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				var path = HttpContext.Current.Server.MapPath("~");
				var FilePath = Path.Combine("E-Forms", e_Forms.Name_EN + "_" + e_Forms.FileName);
				p.E_Forms.Add(new E_Forms() { IS_Action = true, Name = e_Forms.Name, Name_EN = e_Forms.Name_EN, SubServiceID = e_Forms.SubServiceID, Path = FilePath.Replace('\\', '/') });
				File.WriteAllBytes(Path.Combine(path, FilePath), Convert.FromBase64String(e_Forms.Base64));
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
			E_Forms e_Forms = await p.E_Forms.FindAsync(id);
			if (e_Forms == null)
				return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
			e_Forms.IS_Action = true;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Deactive(int id)
		{
			E_Forms e_Forms = await p.E_Forms.FindAsync(id);
			if (e_Forms == null)
				return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
			e_Forms.IS_Action = false;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}

		[HttpPost]
		public async Task<IHttpActionResult> Delete(int id)
		{
			E_Forms e_Forms = p.E_Forms.FirstOrDefault(q => q.ID == id);
			if (e_Forms == null)
				return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
			p.E_Forms.Remove(e_Forms);
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