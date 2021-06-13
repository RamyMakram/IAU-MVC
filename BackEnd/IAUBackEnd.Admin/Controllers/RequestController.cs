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
using System.Web.Http.Cors;
using System.Web.Http.Description;
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
	public class RequestController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		// GET: api/Request
		public async Task<IHttpActionResult> GetRequests_Data(int UserID)
		{
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			if (Unit.IS_Mostafid)
				return Ok(new ResponseClass() { success = true, result = p.Request_Data.Where(q => q.RequestTransaction.Count() == 0 || q.RequestTransaction.Count(w => w.Comment != "" && w.Comment != null) != 0).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, Readed = q.Readed ?? false }) });
			else
				return Ok(new ResponseClass() { success = true, result = p.RequestTransaction.Where(w => w.Comment != "" && w.Comment == null && w.ToUnitID == Unit.Units_ID).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, q.Request_Data.CreatedDate, Readed = q.Request_Data.Readed ?? false }) });
		}
		//[EnableCors(origins: "*", headers: "*", methods: "*")]


		public async Task<IHttpActionResult> GetRequest_Data(int id, int UserID)
		{
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			Request_Data request_Data;
			if (Unit.IS_Mostafid)
				request_Data = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.Request_Data_ID == id && ((q.RequestTransaction.Count == 0 || q.RequestTransaction.Count(w => w.Comment != "" && w.Comment != null) != 0)));
			else
				request_Data = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.Request_Data_ID == id && ((q.RequestTransaction.Count == 0 || q.RequestTransaction.Count(w => (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID) != 0)));
			if (request_Data == null)
				return Ok(new ResponseClass() { success = false });
			if (request_Data.Readed == null || !request_Data.Readed.Value)
			{
				request_Data.Readed = true;
				request_Data.ReadedDate = Helper.GetDate();
				p.SaveChanges();
			}
			return Ok(new ResponseClass() { success = true, result = request_Data });
		}

		// PUT: api/Request/5
		[ResponseType(typeof(void))]
		public async Task<IHttpActionResult> PutRequest_Data(int id, Request_Data request_Data)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != request_Data.Request_Data_ID)
			{
				return BadRequest();
			}

			p.Entry(request_Data).State = EntityState.Modified;

			try
			{
				await p.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!Request_DataExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return StatusCode(HttpStatusCode.NoContent);
		}
		[HttpPost]
		public async Task<IHttpActionResult> SaveApplicantData()
		{
			DbContextTransaction transaction = p.Database.BeginTransaction();
			try
			{
				Request_Data request_Data = new Request_Data();
				var provider = new MultipartMemoryStreamProvider();
				await Request.Content.ReadAsMultipartAsync(provider);
				var buffer = await provider.Contents.Last().ReadAsStringAsync();
				request_Data = JsonConvert.DeserializeObject<Request_Data>(buffer);
				//foreach (var file in provider.Contents)
				//{
				//	if (file.Headers.ContentDisposition.DispositionType.Contains("form-data"))
				//	{
				//		var buffer = await file.ReadAsStringAsync();
				//		requestData = JsonConvert.DeserializeObject<RequestData_DTO>(buffer);
				//	}
				//	else
				//	{
				//		var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
				//		var buffer = await file.ReadAsStringAsync();
				//	}
				//}

				var model = request_Data.Personel_Data;
				Personel_Data personel_Data = p.Personel_Data.FirstOrDefault(q => (q.ID_Document == model.ID_Document && q.ID_Number == model.ID_Number) || q.Mobile == model.Mobile);
				if (personel_Data == null)
				{
					p.Personel_Data.Add(model);
					await p.SaveChangesAsync();
				}
				request_Data.Personel_Data_ID = personel_Data.Personel_Data_ID;
				request_Data.Code_Generate = DateTime.Now.ToString("yyyyMMddHHmm");
				request_Data.CreatedDate = DateTime.Now;
				request_Data.Request_State_ID = 1;
				request_Data.IsTwasul_OC = false;
				request_Data.Readed = false;
				p.Request_Data.Add(request_Data);
				await p.SaveChangesAsync();
				var path = HttpContext.Current.Server.MapPath("~");
				var requestpath = Path.Combine("RequestFiles", request_Data.Request_Data_ID.ToString());
				Directory.CreateDirectory(Path.Combine(path, requestpath));
				if (provider.Contents.Count > 1)
				{
					var count = 0;
					var RequiredFiles = p.Required_Documents.Where(q => q.SubServiceID == request_Data.Sub_Services_ID).ToList();
					if (p.Request_Type.FirstOrDefault(q => q.Request_Type_ID == request_Data.Request_Type_ID).Request_Type_Name_EN.ToLower().Contains("inquiry"))
						foreach (var i in RequiredFiles)
						{
							var file = provider.Contents[count];
							var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
							var Strambuffer = await file.ReadAsByteArrayAsync();
							var filepath = Path.Combine(requestpath, i.Name_EN + "_" + filename);
							File.WriteAllBytes(Path.Combine(path, filepath), Strambuffer);
							request_Data.Request_File.Add(new Request_File()
							{
								Request_ID = request_Data.Request_Data_ID.Value,
								RequiredDoc_ID = i.ID.Value,
								File_Name = filename,
								CreatedDate = DateTime.Now,
								File_Path = filepath.Replace("\\", "/")
							});
							count++;
						}
					int length = provider.Contents.Count - 1;
					for (; count < length; count++)
					{
						var file = provider.Contents[count];
						var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
						var Strambuffer = await file.ReadAsByteArrayAsync();
						var filepath = Path.Combine(requestpath, filename);
						File.WriteAllBytes(Path.Combine(path, filepath), Strambuffer);
						p.Request_File.Add(new Request_File()
						{
							Request_ID = request_Data.Request_Data_ID.Value,
							CreatedDate = DateTime.Now,
							File_Name = filename,
							File_Path = filepath.Replace("\\", "/")
						});
					}

				}
				p.SaveChanges();
				Request_Data sendeddata = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == request_Data.Request_Data_ID);

				var MostafidUsers = p.Users.Where(q => q.Units.IS_Mostafid).Select(q => q.User_ID).ToArray();
				string message = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
				WebSocketManager.SendToMulti(MostafidUsers, message);
				message = @"عزيزي المستفيد ، 
									تم استلام طلبكم بنجاح ، وسيتم افادتكم بالكود الخاص بالطلب خلال ٤٨ ساعه";
				SendSMS(model.Mobile, message);
				transaction.Commit();
				return Ok(new
				{
					success = true
				});
			}
			catch (Exception e)
			{
				transaction.Rollback();
				return Ok(new
				{
					success = false
				});
			}
		}

		[HttpGet]
		[Route("api/Request/SendSMS")]
		public async Task<IHttpActionResult> SendSMS(string Mobile, string message)
		{
			try
			{
				HttpClient h = new HttpClient();

				string url = $"http://basic.unifonic.com/wrapper/sendSMS.php?appsid=f9iRotRBsanfAB0xcE4NzJtgMYf5Bk&msg={message}&to={Mobile}&sender=IAU-BSC&baseEncode=False&encoding=UCS2";
				h.BaseAddress = new Uri(url);

				var res = h.GetAsync("").Result.Content.ReadAsStringAsync().Result;
				return Ok(new ResponseClass()
				{
					result = res,
					success = true
				});
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass()
				{
					result = ee,
					success = false
				});
			}
		}
		[HttpGet]
		public async Task<IHttpActionResult> Coding(int RequestIID, bool IsTwasul_OC, int Service_Type_ID, int Request_Type_ID, int? locations, string BuildingSelect, int Unit_ID, string type)
		{
			var req = p.Request_Data.Include(q => q.Personel_Data).FirstOrDefault(q => q.Request_Data_ID == RequestIID);
			req.IsTwasul_OC = IsTwasul_OC;
			req.Service_Type_ID = Service_Type_ID;
			req.Request_Type_ID = Request_Type_ID;
			req.Unit_ID = Unit_ID;
			string Code = GetCode(RequestIID, IsTwasul_OC, Service_Type_ID, Request_Type_ID, locations, BuildingSelect, Unit_ID, type);
			if (req.Code_Generate == "" || req.Code_Generate == null)
			{
				req.Code_Generate = Code;
				p.SaveChanges();
				if (type == "c")
					_ = SendSMS(req.Personel_Data.Mobile, $"تم استلام طلبكم رقم {req.Request_Data_ID} برجاء استخدام الكود ${Code} في حالة الاستعلام ع");
			}
			else
				req.TempCode = Code;
			p.SaveChanges();
			return Ok(new ResponseClass() { success = true });
		}

		[HttpGet]
		public async Task<IHttpActionResult> Forward(int RequestIID, int Unit_ID, Nullable<DateTime> Expected)
		{
			try
			{
				p.RequestTransaction.Add(new RequestTransaction() { Request_ID = RequestIID, ExpireDays = Expected, ForwardDate = Helper.GetDate(), ToUnitID = Unit_ID, Readed = false, FromUnitID = p.Units.First(q => q.IS_Mostafid).Units_ID });
				p.SaveChanges();
				return Ok(new ResponseClass() { success = true });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false });
			}
		}

		[HttpGet]
		public async Task<IHttpActionResult> GenrateCode(int RequestIID, bool IsTwasul_OC, int Service_Type_ID, int Request_Type_ID, int? locations, string BuildingSelect, int Unit_ID, string type)
		{
			string Code = GetCode(RequestIID, IsTwasul_OC, Service_Type_ID, Request_Type_ID, locations, BuildingSelect, Unit_ID, type);
			return Ok(new ResponseClass() { success = true, result = Code });
		}
		private string GetCode(int RequestIID, bool IsTwasul_OC, int Service_Type_ID, int Request_Type_ID, int? locations, string BuildingSelect, int Unit_ID, string type)
		{
			var unit = p.Units.Include(q => q.Units_Location).FirstOrDefault(q => q.Units_ID == Unit_ID);
			int? loc = (locations ?? unit.Units_Location.Location_ID);
			var Location = p.Units_Location.FirstOrDefault(q => q.Location_ID == loc).Code;
			string Code = (IsTwasul_OC ? "2" : "1");
			Code += Location;
			Code += (BuildingSelect == null || BuildingSelect == "null") ? unit.Building_Number : BuildingSelect;
			Code += Service_Type_ID + "" + Request_Type_ID;
			Code += (type == "c" ? "00000" : string.Join("0", new string[5 - RequestIID.ToString().Length]) + RequestIID);
			return Code;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				p.Dispose();
			base.Dispose(disposing);
		}

		private bool Request_DataExists(int id)
		{
			return p.Request_Data.Count(e => e.Request_Data_ID == id) > 0;
		}
	}
}