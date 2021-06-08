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
		public async Task<IHttpActionResult> GetRequest_Data()
		{
			return Ok(new ResponseClass() { success = true, result = p.Request_Data.Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate }) });
		}
		[EnableCors(origins: "*", headers: "*", methods: "*")]

		public async Task<IHttpActionResult> GetRequest_PDF(string ID)
		{
			return Ok(new ResponseClass() { success = true, result = File.ReadAllText(HttpContext.Current.Server.MapPath("~/RequestFiles/" + ID + "/PDF.txt")) });
		}

		public async Task<IHttpActionResult> GetRequest_Data(int id)
		{
			Request_Data request_Data = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == id);
			if (request_Data == null)
			{
				return NotFound();
			}

			return Ok(request_Data);
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
		// POST: api/Request
		[ResponseType(typeof(Request_Data))]
		public async Task<IHttpActionResult> PostRequest_Data(Request_Data request_Data)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			p.Request_Data.Add(request_Data);
			await p.SaveChangesAsync();

			return CreatedAtRoute("DefaultApi", new { id = request_Data.Request_Data_ID }, request_Data);
		}

		// DELETE: api/Request/5
		[ResponseType(typeof(Request_Data))]
		public async Task<IHttpActionResult> DeleteRequest_Data(int id)
		{
			Request_Data request_Data = await p.Request_Data.FindAsync(id);
			if (request_Data == null)
			{
				return NotFound();
			}

			p.Request_Data.Remove(request_Data);
			await p.SaveChangesAsync();

			return Ok(request_Data);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				p.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool Request_DataExists(int id)
		{
			return p.Request_Data.Count(e => e.Request_Data_ID == id) > 0;
		}
	}
}