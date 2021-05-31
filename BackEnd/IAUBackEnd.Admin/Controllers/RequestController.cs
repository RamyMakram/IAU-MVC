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
		[Route("api/Request/saveApplicantData")]
		public async Task<IHttpActionResult> SaveApplicantData(RequestData_DTO requestData)
		{
			try
			{
				var data = JsonConvert.DeserializeObject<Request_Data>(JsonConvert.SerializeObject(requestData.Request));
				var model = data.Personel_Data;
				Personel_Data personel_Data = p.Personel_Data.FirstOrDefault(q => (q.ID_Document == model.ID_Document && q.ID_Number == model.ID_Number) || q.Mobile == model.Mobile);
				if (personel_Data == null)
				{
					p.Personel_Data.Add(model);
					await p.SaveChangesAsync();
				}

				//save request data
				//byte request_State_ID = (byte)RequestState.Created;

				Request_Data request_Data = data;
				request_Data.Personel_Data_ID = personel_Data.Personel_Data_ID;
				request_Data.Code_Generate = DateTime.Now.ToString("yyyyMMddHHmm");
				request_Data.CreatedDate = DateTime.Now;
				request_Data.Request_State_ID = 1;
				//IsTwasul=true, OC ON-CAMPUS بالمركز=false
				request_Data.IsTwasul_OC = false;
				p.Request_Data.Add(request_Data);
				await p.SaveChangesAsync();
				var path = HttpContext.Current.Server.MapPath("~");
				var requestpath = Path.Combine("RequestFiles", request_Data.Request_Data_ID.ToString());
				Directory.CreateDirectory(Path.Combine(path, requestpath));
				var count = 0;
				var RequiredFiles = p.Required_Documents.ToList();
				if (p.Request_Type.FirstOrDefault(q => q.Request_Type_ID == request_Data.Request_Type_ID).Request_Type_Name_EN.ToLower().Contains("inquiry"))
					foreach (var i in RequiredFiles)
					{
						var filename = Path.GetFileName(requestData.Files[count].filename);
						var filepath = Path.Combine(requestpath, i.Name_EN + "_" + filename);
						File.WriteAllBytes(Path.Combine(path, filepath), requestData.Files[count].bytes);
						request_Data.Request_File.Add(new Request_File()
						{
							Request_ID = request_Data.Request_Data_ID,
							RequiredDoc_ID = i.ID.Value,
							File_Name = filename,
							CreatedDate = DateTime.Now,
							File_Path = filepath.Replace("\\", "/")
						});
						count++;
					}
				int length = requestData.Files.Count;
				for (; count < length; count++)
				{
					var filename = Path.GetFileName(requestData.Files[count].filename);
					var filepath = Path.Combine(requestpath, filename);
					File.WriteAllBytes(Path.Combine(path, filepath), requestData.Files[count].bytes);
					p.Request_File.Add(new Request_File()
					{
						Request_ID = request_Data.Request_Data_ID,
						CreatedDate = DateTime.Now,
						File_Name = filename,
						File_Path = filepath.Replace("\\", "/")
					});
				}

				p.SaveChanges();
				var PDFPath = Path.Combine(requestpath, "PDF.txt");
				Request_Data sendeddata = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == request_Data.Request_Data_ID);

				var MostafidUsers = p.Users.Where(q => q.Units.IS_Mostafid).Select(q => q.User_ID).ToArray();
				string message = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
				WebSocketManager.SendToMulti(MostafidUsers, message);
				message = @"عزيزي المستفيد ، 
									تم استلام طلبكم بنجاح ، وسيتم افادتكم بالكود الخاص بالطلب خلال ٤٨ ساعه";
				_ = SendSMS(model.Mobile, message);
				return Ok(new
				{
					success = true
				});
			}
			catch (Exception e)
			{
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