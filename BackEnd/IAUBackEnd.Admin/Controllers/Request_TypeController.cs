﻿using System;
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
	public class Request_TypeController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		public async Task<IHttpActionResult> GetRequest_Type()
		{
			return Ok(new ResponseClass() { success = true, result = p.Request_Type });
		}
		public async Task<IHttpActionResult> GetActive()
		{
			return Ok(new ResponseClass() { success = true, result = p.Request_Type.Where(q => q.IS_Action.Value) });
		}
		public async Task<IHttpActionResult> GetActiveByMainService(int SID)
		{
			return Ok(new ResponseClass() { success = true, result = p.Request_Type.Where(q => q.IS_Action.Value && q.Units_Request_Type.Count(w => (w.Units.ServiceTypeID == SID || w.Units.UnitServiceTypes.Count(s => s.ServiceTypeID == SID) != 0) && w.Units.UnitMainServices.Count(r => r.Main_Services.IS_Action.Value && r.Main_Services.Sub_Services.Count(g => g.IS_Action.Value) != 0) != 0) != 0).Select(q => new { ID = q.Request_Type_ID, Name_AR = q.Request_Type_Name_AR, Name_EN = q.Request_Type_Name_EN, q.Image_Path }) });
		}
		public async Task<IHttpActionResult> GetRequest_Type(int id)
		{
			Request_Type request_Type = await p.Request_Type.FindAsync(id);
			if (request_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });

			return Ok(new ResponseClass() { success = true, result = request_Type });
		}

		public async Task<IHttpActionResult> Edit(Request_Type request_Type)
		{
			var data = p.Request_Type.FirstOrDefault(q => q.Request_Type_ID == request_Type.Request_Type_ID);
			if (!ModelState.IsValid || data == null)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				data.Request_Type_Name_AR = request_Type.Request_Type_Name_AR;
				data.Request_Type_Name_EN = request_Type.Request_Type_Name_EN;
				await p.SaveChangesAsync();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> Create(RequestTypeDTO request_Type)
		{
			if (!ModelState.IsValid)
				return Ok(new ResponseClass() { success = false, result = ModelState });
			try
			{
				var path = HttpContext.Current.Server.MapPath("~");
				var FilePath = Path.Combine("Images", "Request_Type", DateTime.Now.Ticks.ToString() + ".png");
				p.Request_Type.Add(new Request_Type() { IS_Action = true, Request_Type_Name_AR = request_Type.Request_Type_Name_AR, Request_Type_Name_EN = request_Type.Request_Type_Name_EN, Image_Path = FilePath.Replace('\\', '/') });
				File.WriteAllBytes(Path.Combine(path, FilePath), Convert.FromBase64String(request_Type.Base64));
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
			Request_Type request_Type = p.Request_Type.FirstOrDefault(q => q.Request_Type_ID == id);
			if (request_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });
			request_Type.IS_Action = false;
			await p.SaveChangesAsync();

			return Ok(new ResponseClass() { success = true });
		}
		[HttpGet]
		public async Task<IHttpActionResult> Active(int id)
		{
			Request_Type request_Type = p.Request_Type.FirstOrDefault(q => q.Request_Type_ID == id);
			if (request_Type == null)
				return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });
			request_Type.IS_Action = true;
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