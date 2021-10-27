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
using LinqKit;
using Newtonsoft.Json;
using System.Linq.Dynamic;
using System.Net.Mail;

namespace IAUBackEnd.Admin.Controllers
{
	public class RequestController : ApiController
	{
		private MostafidDBEntities p = new MostafidDBEntities();

		// GET: api/Request
		public async Task<IHttpActionResult> GetRequests_Data(int UserID)
		{
			try
			{
				var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
				if (Unit.IS_Mostafid)
				{
					var data = p.Request_Data.Where(q => !q.Is_Archived && q.Request_State_ID != 5 && (q.RequestTransaction.Count() == 0 || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment != null)).Select(q => new { Required_Fields_Notes = q.RequestTransaction.Count() == 0 ? q.Required_Fields_Notes : q.RequestTransaction.OrderByDescending(s => s.CommentDate).FirstOrDefault().Comment, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, CreatedDate = q.RequestTransaction.Count == 0 ? q.CreatedDate : q.RequestTransaction.OrderByDescending(ssd => ssd.ID).FirstOrDefault().CommentDate, Readed = q.Readed ?? false, q.Request_State_ID, }).OrderByDescending(q => q.Request_Data_ID);
					var ss = data.ToList();
					return Ok(new ResponseClass() { success = true, result = data });
				}
				else
				{
					var data = p.RequestTransaction.Where(w => !w.Request_Data.Is_Archived && (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID && w.Request_Data.Request_State_ID != 5).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, CreatedDate = q.ForwardDate, q.Readed }).OrderByDescending(q => q.CreatedDate);
					var ss = data.ToList();
					return Ok(new ResponseClass() { success = true, result = data });
				}
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> GetFilterdRequests_Data(int? ST, int? RT, int? MT, DateTime? DF, DateTime? DT, int UserID)
		{
			try
			{
				var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
				if (Unit.IS_Mostafid)
				{
					var Pred = PredicateBuilder.New<Request_Data>(q => !q.Is_Archived && q.Request_State_ID != 5 && (q.RequestTransaction.Count() == 0 || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment != null));
					if (ST.HasValue)
						Pred.And(q => q.Service_Type_ID == ST);
					if (RT.HasValue)
						Pred.And(q => q.Request_Type_ID == RT);
					if (MT.HasValue)
						if (MT == 0)
							Pred.And(q => q.Personel_Data.IAU_ID_Number == "" || q.Personel_Data.IAU_ID_Number != null);
						else
							Pred.And(q => q.Personel_Data.IAU_ID_Number != "" && q.Personel_Data.IAU_ID_Number != null);
					if (DF.HasValue)
						Pred.And(q => q.CreatedDate >= DF);
					if (DT.HasValue)
						Pred.And(q => q.CreatedDate <= DT);

					var data = p.Request_Data.Where(Pred).Select(q => new { Required_Fields_Notes = q.RequestTransaction.Count() == 0 ? q.Required_Fields_Notes : q.RequestTransaction.OrderByDescending(s => s.CommentDate).FirstOrDefault().Comment, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, CreatedDate = q.RequestTransaction.Count == 0 ? q.CreatedDate : q.RequestTransaction.OrderByDescending(ss => ss.ID).FirstOrDefault().CommentDate, Readed = q.Readed ?? false, q.Request_State_ID, }).OrderByDescending(q => q.Request_Data_ID);
					return Ok(new ResponseClass() { success = true, result = data });
				}
				else
				{
					var Pred = PredicateBuilder.New<RequestTransaction>(w => !w.Request_Data.Is_Archived && (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID && w.Request_Data.Request_State_ID != 5);
					if (ST.HasValue)
						Pred.And(q => q.Request_Data.Service_Type_ID == ST);
					if (RT.HasValue)
						Pred.And(q => q.Request_Data.Request_Type_ID == RT);
					if (MT.HasValue)
						if (MT == 0)
							Pred.And(q => q.Request_Data.Personel_Data.IAU_ID_Number == "" || q.Request_Data.Personel_Data.IAU_ID_Number != null);
						else
							Pred.And(q => q.Request_Data.Personel_Data.IAU_ID_Number != "" && q.Request_Data.Personel_Data.IAU_ID_Number != null);
					if (DF.HasValue)
						Pred.And(q => q.Request_Data.CreatedDate >= DF);
					if (DT.HasValue)
						Pred.And(q => q.Request_Data.CreatedDate <= DT);

					var data = p.RequestTransaction.Where(Pred).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, CreatedDate = q.ForwardDate, q.Readed }).OrderByDescending(q => q.CreatedDate);
					return Ok(new ResponseClass() { success = true, result = data });
				}
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> GetSendedRequests_Data(int UserID)
		{
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			if (Unit.IS_Mostafid)
			{
				var data = p.Request_Data.Where(q => !q.Is_Archived && q.Request_State_ID != 5 && q.RequestTransaction.Count() != 0 && (q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment == "" || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment == null)).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
				return Ok(new ResponseClass() { success = true, result = data });
			}
			else
			{
				var data = p.RequestTransaction.Where(w => !w.Request_Data.Is_Archived && (w.Comment != "" && w.Comment != null) && w.ToUnitID == Unit.Units_ID && w.Request_Data.Request_State_ID != 5).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, q.Request_Data.CreatedDate, q.Request_Data.Readed }).Distinct().OrderByDescending(q => q.CreatedDate);
				return Ok(new ResponseClass() { success = true, result = data });
			}
		}
		public async Task<IHttpActionResult> GetFilterSendedRequests_Data(int? ST, int? RT, int? MT, DateTime? DF, DateTime? DT, int UserID)
		{
			try
			{
				var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
				if (Unit.IS_Mostafid)
				{
					var Pred = PredicateBuilder.New<Request_Data>(q => !q.Is_Archived && q.Request_State_ID != 5 && q.RequestTransaction.Count() != 0 && (q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment == "" || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment == null));
					if (ST.HasValue)
						Pred.And(q => q.Service_Type_ID == ST);
					if (RT.HasValue)
						Pred.And(q => q.Request_Type_ID == RT);
					if (MT.HasValue)
						if (MT == 0)
							Pred.And(q => q.Personel_Data.IAU_ID_Number == "" || q.Personel_Data.IAU_ID_Number != null);
						else
							Pred.And(q => q.Personel_Data.IAU_ID_Number != "" && q.Personel_Data.IAU_ID_Number != null);
					if (DF.HasValue)
						Pred.And(q => q.CreatedDate >= DF);
					if (DT.HasValue)
						Pred.And(q => q.CreatedDate <= DT);

					var data = p.Request_Data.Where(Pred).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
					return Ok(new ResponseClass() { success = true, result = data });
				}
				else
				{
					var Pred = PredicateBuilder.New<RequestTransaction>(w => !w.Request_Data.Is_Archived && (w.Comment != "" && w.Comment != null) && w.ToUnitID == Unit.Units_ID && w.Request_Data.Request_State_ID != 5);
					if (ST.HasValue)
						Pred.And(q => q.Request_Data.Service_Type_ID == ST);
					if (RT.HasValue)
						Pred.And(q => q.Request_Data.Request_Type_ID == RT);
					if (MT.HasValue)
						if (MT == 0)
							Pred.And(q => q.Request_Data.Personel_Data.IAU_ID_Number == "" || q.Request_Data.Personel_Data.IAU_ID_Number != null);
						else
							Pred.And(q => q.Request_Data.Personel_Data.IAU_ID_Number != "" && q.Request_Data.Personel_Data.IAU_ID_Number != null);
					if (DF.HasValue)
						Pred.And(q => q.Request_Data.CreatedDate >= DF);
					if (DT.HasValue)
						Pred.And(q => q.Request_Data.CreatedDate <= DT);

					var data = p.RequestTransaction.Where(Pred).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, q.Request_Data.CreatedDate, q.Request_Data.Readed }).Distinct().OrderByDescending(q => q.CreatedDate);
					return Ok(new ResponseClass() { success = true, result = data });
				}
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
		public async Task<IHttpActionResult> GetArchivedRequests_Data(int UserID)
		{
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			if (Unit.IS_Mostafid)
			{
				var data = p.Request_Data.Where(q => q.Is_Archived).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
				return Ok(new ResponseClass() { success = true, result = data });
			}
			return Ok(new ResponseClass() { success = false });
		}

		public async Task<IHttpActionResult> GetFilterArchivedRequests_Data(int? ST, int? RT, int? MT, DateTime? DF, DateTime? DT, int UserID)
		{
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			if (Unit.IS_Mostafid)
			{
				var Pred = PredicateBuilder.New<Request_Data>(q => q.Is_Archived);
				if (ST.HasValue)
					Pred.And(q => q.Service_Type_ID == ST);
				if (RT.HasValue)
					Pred.And(q => q.Request_Type_ID == RT);
				if (MT.HasValue)
					if (MT == 0)
						Pred.And(q => q.Personel_Data.IAU_ID_Number == "" || q.Personel_Data.IAU_ID_Number != null);
					else
						Pred.And(q => q.Personel_Data.IAU_ID_Number != "" && q.Personel_Data.IAU_ID_Number != null);
				if (DF.HasValue)
					Pred.And(q => q.CreatedDate >= DF);
				if (DT.HasValue)
					Pred.And(q => q.CreatedDate <= DT);

				var data = p.Request_Data.Where(Pred).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
				return Ok(new ResponseClass() { success = true, result = data });
			}
			return Ok(new ResponseClass() { success = false });
		}

		public async Task<IHttpActionResult> GetRequestsCount(int UserID)
		{
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			if (Unit.IS_Mostafid)
				return Ok(new ResponseClass() { success = true, result = p.Request_Data.Count(q => !q.Is_Archived && q.Request_State_ID != 5 && (q.RequestTransaction.Count() == 0 || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment != null)) });
			else
				return Ok(new ResponseClass() { success = true, result = p.RequestTransaction.Count(w => !w.Request_Data.Is_Archived && (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID && !w.Readed) });
		}

		//[EnableCors(origins: "*", headers: "*", methods: "*")]

		public async Task<IHttpActionResult> GetRequest_Data(int id, int UserID)
		{
			try
			{
				var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
				Request_Data request_Data;
				if (Unit.IS_Mostafid)
					request_Data = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Units).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.Request_State_ID != 5 && q.Request_Data_ID == id && (q.RequestTransaction.Count == 0 || q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Comment != null));
				else
					request_Data = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Units).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents))
						.FirstOrDefault(q => q.Request_Data_ID == id && ((q.RequestTransaction.Count == 0 || q.RequestTransaction.Count(w => (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID) != 0)));
				if (request_Data == null)
					return Ok(new ResponseClass() { success = false });
				if (Unit.IS_Mostafid)
				{
					if (request_Data.Readed == null || !request_Data.Readed.Value)
					{
						request_Data.Readed = true;
						request_Data.ReadedDate = Helper.GetDate();
						p.SaveChanges();
					}
				}
				else if (!Unit.IS_Mostafid)
				{
					var data = request_Data.RequestTransaction.ToList().Last();
					data.Readed = true;
					if (request_Data.Request_State_ID == 2)
						request_Data.Request_State_ID = 3;
					p.SaveChanges();
				}
				return Ok(new ResponseClass() { success = true, result = new { request_Data, request_Data.Units.Units_Location_ID, request_Data.Units.Building_Number } });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
		public async Task<IHttpActionResult> GetSendedRequest_Data(int id, int UserID)
		{
			try
			{
				var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
				Request_Data request_Data;
				if (Unit.IS_Mostafid)
					request_Data = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.Request_State_ID != 5 && q.Request_Data_ID == id && (q.RequestTransaction.Count == 0 || q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Comment == null));
				else
					request_Data = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents))
						.FirstOrDefault(q => q.Request_Data_ID == id && ((q.RequestTransaction.Count != 0 || q.RequestTransaction.Count(w => (w.Comment != "" && w.Comment != null) && w.ToUnitID == Unit.Units_ID) != 0)));
				if (request_Data == null)
					return Ok(new ResponseClass() { success = false });

				return Ok(new ResponseClass() { success = true, result = request_Data });
			}
			catch (Exception ee)
			{

				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
		public async Task<IHttpActionResult> GetArchivedRequest_Data(int id, int UserID)
		{
			try
			{
				var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
				if (Unit.IS_Mostafid)
				{
					Request_Data request_Data = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).Include(q => q.Units).FirstOrDefault(q => q.Is_Archived && q.Request_Data_ID == id);
					if (request_Data == null)
						return Ok(new ResponseClass() { success = false });
					return Ok(new ResponseClass() { success = true, result = request_Data });
				}
				return Ok(new ResponseClass() { success = false });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}

		public async Task<IHttpActionResult> GetRequestTranscation(int id, int UserID)
		{
			var RequestTransaction = p.RequestTransaction.Include(q => q.Units1).Where(q => q.Request_ID == id);
			return Ok(new ResponseClass() { success = true, result = RequestTransaction });
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

				var model = request_Data.Personel_Data;
				Personel_Data personel_Data = p.Personel_Data.FirstOrDefault(q => (q.ID_Document == model.ID_Document && q.ID_Number == model.ID_Number) || q.Mobile == model.Mobile);
				if (personel_Data == null)
				{
					p.Personel_Data.Add(model);
					await p.SaveChangesAsync();
					request_Data.Personel_Data_ID = model.Personel_Data_ID;
				}
				else
					request_Data.Personel_Data_ID = personel_Data.Personel_Data_ID;
				request_Data.CreatedDate = Helper.GetDate();
				request_Data.Request_State_ID = 1;
				request_Data.IsTwasul_OC = false;
				request_Data.Readed = false;
				request_Data.Is_Archived = false;
				request_Data.Request_State_ID = 1;
				p.Request_Data.Add(request_Data);
				await p.SaveChangesAsync();
				var path = HttpContext.Current.Server.MapPath("~");
				var requestpath = Path.Combine("RequestFiles", request_Data.Request_Data_ID.ToString());
				Directory.CreateDirectory(Path.Combine(path, requestpath));
				if (provider.Contents.Count > 1)
				{
					var count = 0;
					if (p.Request_Type.FirstOrDefault(q => q.Request_Type_ID == request_Data.Request_Type_ID).Request_Type_Name_EN.ToLower().Contains("inquiry"))
					{
						var RequiredFiles = p.Required_Documents.Where(q => q.SubServiceID == request_Data.Sub_Services_ID).ToList();
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
				_ = NotifyUser(model.Mobile, model.Email, @"عزيزي المستفيد ، تم استلام طلبكم بنجاح ، وسيتم افادتكم بالكود الخاص بالطلب خلال ٤٨ ساعة", @"Dear Mostafid, your order has been successfully received, and you will be notified of the order code within 48 hours");
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
		[Route("api/Request/NotifyUser")]
		public async Task<IHttpActionResult> NotifyUser(string Mobile, string Email, string message_ar, string message_en)
		{
			try
			{
				if (WebApiApplication.Setting_UseMessage)
				{
					HttpClient h = new HttpClient();

					string url = $"http://basic.unifonic.com/wrapper/sendSMS.php?appsid=f9iRotRBsanfAB0xcE4NzJtgMYf5Bk&msg={message_ar}&to={Mobile}&sender=IAU-BSC&baseEncode=False&encoding=UCS2";
					h.BaseAddress = new Uri(url);

					var res = h.GetAsync("").Result.Content.ReadAsStringAsync().Result;
				}
				var message = $@"
					<p dir='ltr'>{message_en}</p>
					<p dir='rtl'>{message_ar}</p>
					";
				//SmtpClient smtpClient = new SmtpClient("mail.iau.edu.sa", 25);

				//smtpClient.Credentials = new System.Net.NetworkCredential("noreply.bsc@iau.edu.sa", "Bsc@33322");
				//// smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
				//smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				////smtpClient.EnableSsl = true;
				//MailMessage mail = new MailMessage();

				////Setting From , To and CC
				//mail.From = new MailAddress("noreply.bsc@iau.edu.sa", "Mustafid");
				//mail.To.Add(new MailAddress(Email));
				//mail.Subject = "IAU Notify";
				//mail.Body = message;
				//smtpClient.Send(mail);
				SmtpClient smtpClient = new SmtpClient("mail.iau-bsc.com", 25);

				smtpClient.Credentials = new System.Net.NetworkCredential("ramy@iau-bsc.com", "ENGGGGAAA1448847@");
				// smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
				smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
				//smtpClient.EnableSsl = true;
				MailMessage mail = new MailMessage();

				//Setting From , To and CC
				mail.From = new MailAddress("ramy@iau-bsc.com", "Mustafid");
				mail.To.Add(new MailAddress(Email));
				mail.Subject = "IAU Notify";
				mail.Body = message;
				mail.IsBodyHtml = true;
				smtpClient.Send(mail);
				return Ok(new ResponseClass()
				{
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
			if (req.TempCode == "" || req.TempCode == null)
			{
				req.IsTwasul_OC = IsTwasul_OC;
				req.Service_Type_ID = Service_Type_ID;
				req.Request_Type_ID = Request_Type_ID;
				req.Unit_ID = Unit_ID;
				string Code = GetCode(RequestIID, IsTwasul_OC, Service_Type_ID, Request_Type_ID, locations, BuildingSelect, Unit_ID, type);
				if (req.Code_Generate == "" || req.Code_Generate == null)
				{
					req.Code_Generate = Code;
					req.TempCode = Code;
					req.GenratedDate = Helper.GetDate();
					p.SaveChanges();
					string message = $@"عزيزي المستفيد, :نفيدكم بأن كود الطلب الخاص بكم هو {Code} برجاء استخدامة في حالة الاستعلام";
					_ = NotifyUser(req.Personel_Data.Mobile, req.Personel_Data.Email, $@"برجاء استخدامة في حالة الاستعلام ، '{Code}' : عزيزي المستفيد،نفيدكم بأن كود الطلب الخاص بكم هو", $@"Dear Mostafid, we inform you that your request code is: '{Code}' Please use it in case of query");
				}
				else
				{
					req.TempCode = Code;
					req.GenratedDate = Helper.GetDate();
				}
				p.SaveChanges();
				return Ok(new ResponseClass() { success = true });
			}
			else
				return Ok(new ResponseClass() { success = false });

		}

		[HttpPost]
		public async Task<IHttpActionResult> Forward(int RequestIID, int Unit_ID, Nullable<DateTime> Expected, [FromBody] string comment)
		{
			try
			{
				Request_Data sendeddata = p.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == RequestIID);
				if (sendeddata.TempCode != "")
				{
					p.RequestTransaction.Add(new RequestTransaction() { Request_ID = RequestIID, ExpireDays = Expected, ForwardDate = Helper.GetDate(), ToUnitID = Unit_ID, Readed = false, FromUnitID = p.Units.First(q => q.IS_Mostafid).Units_ID, Code = sendeddata.TempCode, MostafidComment = comment, Is_Reminder = false });
					sendeddata.TempCode = "";
					if (sendeddata.Request_State_ID == 1)
						sendeddata.Request_State_ID = 2;
					p.SaveChanges();
					sendeddata.Readed = false;
					var Users = p.Users.Where(q => q.Units.Units_ID == Unit_ID).Select(q => q.User_ID).ToArray();
					string message = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
					WebSocketManager.SendToMulti(Users, message);
					return Ok(new ResponseClass() { success = true });
				}
				else
					return Ok(new ResponseClass() { success = false });

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

		[HttpPost]
		public async Task<IHttpActionResult> AddComment(int UserID, int RequestID, int CommentType, [FromBody] string Comment)
		{
			var CurrentUnit = p.Users.FirstOrDefault(q => q.User_ID == UserID).UnitID;
			Request_Data sendeddata = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == RequestID);
			var trans = sendeddata.RequestTransaction.Last();
			if (CurrentUnit == trans.ToUnitID)
			{
				trans.Comment = Comment;
				trans.CommentDate = Helper.GetDate();
				trans.CommentType = CommentType;
				if (sendeddata.Request_State_ID == 3)
					sendeddata.Request_State_ID = 4;
				sendeddata.Readed = false;
				p.SaveChanges();
				var Users = p.Users.Where(q => q.Units.IS_Mostafid).Select(q => q.User_ID).ToArray();
				string message = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
				WebSocketManager.SendToMulti(Users, message);
				return Ok(new ResponseClass() { success = true });
			}
			else
				return Ok(new ResponseClass() { success = false });
		}

		[HttpPost]
		public async Task<IHttpActionResult> AddReminder(int UserID, int RequestID, [FromBody] string Comment)
		{
			var IsMostafid = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			Request_Data sendeddata = p.Request_Data.FirstOrDefault(q => q.Request_Data_ID == RequestID);
			var trans = p.RequestTransaction.Where(q => q.Request_ID == RequestID).OrderByDescending(q => q.ID).FirstOrDefault();
			if (IsMostafid.IS_Mostafid)
			{
				trans.Comment = "Delayed";
				p.RequestTransaction.Add(new RequestTransaction() { Request_ID = RequestID, ForwardDate = Helper.GetDate(), ToUnitID = trans.ToUnitID, Readed = false, FromUnitID = IsMostafid.Units_ID, Code = trans.Code, MostafidComment = Comment, Is_Reminder = true });
				sendeddata.Readed = false;
				p.SaveChanges();
				var Users = p.Users.Where(q => q.Units.Units_ID == trans.ToUnitID).Select(q => q.User_ID).ToArray();
				string message = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
				WebSocketManager.SendToMulti(Users, message);
				return Ok(new ResponseClass() { success = true });
			}
			else
				return Ok(new ResponseClass() { success = false });
		}

		[HttpPost]
		public async Task<IHttpActionResult> ArchiveRequests(int UserID, [FromBody] string Requests)
		{
			var requests = JsonConvert.DeserializeObject<int[]>(Requests);
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;
			if (Unit.IS_Mostafid)
			{
				foreach (var i in requests)
				{
					Request_Data sendeddata = p.Request_Data.FirstOrDefault(q => q.Request_Data_ID == i);
					if (sendeddata.Request_State_ID == 1)
					{
						sendeddata.Request_State_ID = 5;
						sendeddata.Is_Archived = true;
					}
				}
				p.SaveChanges();
				return Ok(new ResponseClass() { success = true });
			}
			return Ok(new ResponseClass() { success = false });
		}

		[HttpPost]
		public async Task<IHttpActionResult> CloseRequest(int UserID, int RequestID)
		{
			Request_Data sendeddata = p.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Personel_Data).FirstOrDefault(q => q.Request_Data_ID == RequestID);
			var Unit = p.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID).Units;

			if (Unit.IS_Mostafid && (sendeddata.Request_State_ID != 2))
			{
				sendeddata.Request_State_ID = 5;
				sendeddata.Is_Archived = true;
				p.SaveChanges();
				string message = $@"عزيزي المستفيد , تم الانتهاء من الطلب  رقم {sendeddata.Code_Generate}";
				if (sendeddata.RequestTransaction.Count != 0)
					_ = NotifyUser(sendeddata.Personel_Data.Mobile, sendeddata.Personel_Data.Email, $@"'{sendeddata.Code_Generate}' عزيزي المستفيد، تم الانتهاء من الطلب رقم", $"Dear Mostafid, Request number '{sendeddata.Code_Generate}' has been completed");
				return Ok(new ResponseClass() { success = true });
			}
			return Ok(new ResponseClass() { success = false });
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
			Code += (string.Join("0", new string[5 - RequestIID.ToString().Length + 1]) + RequestIID);
			if (Code.Length != 13)
				Code += string.Join("0", new string[13 - Code.Length]);
			return Code;
		}
		[HttpPost]
		public async Task<IHttpActionResult> FollowRequest([FromBody] string Code)
		{
			try
			{
				var data = p.Request_Data.Include(q => q.Units).Include(q => q.Request_State).Where(q => q.Code_Generate == Code)
					.Select(q => new { q.IsTwasul_OC, q.Request_State, q.Request_Data_ID, q.Request_State_ID }).FirstOrDefault();
				return Ok(new ResponseClass() { success = true, result = new { Request = data, State = p.RequestTransaction.Where(q => q.Request_ID == data.Request_Data_ID).Include(q => q.Units).OrderByDescending(w => w.ID).FirstOrDefault() } });
			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
		public string SelectQueryData(string cols)
		{
			var ReturnedXols = new[] {
				new { Table = "Personel_Data" ,Cols=new string[]{ "IAU_ID_Number", "Middle_Name", "Last_Name", "ID_Number", "First_Name", "Mobile", "Email", "Postal_Code" } },
				new { Table = "Personel_Data.Applicant_Type" ,Cols =new string[]{ "ApplicantType" } },
				//new { Table = "Personel_Data.Title_Middle_Names" ,Cols =new string[]{ "Title_Middle_Names_ID"} },
				new { Table = "Personel_Data.Country" ,Cols =new string[]{ "Nationality" } },
				new { Table = "Personel_Data.Country2" ,Cols =new string[]{ "Country" } },
				new { Table = "Personel_Data.City" ,Cols =new string[]{ "City" } },
				new { Table = "Personel_Data.Region" ,Cols =new string[]{ "Region" } },
				new { Table = "" ,Cols =new string[]{ "Code_Generate", "CreatedDate" } },
				new { Table = "Service_Type" ,Cols =new string[]{ "ServiceType" } },
				new { Table = "Request_Type" ,Cols =new string[]{ "RequestType" } },
				new { Table = "Request_State" ,Cols =new string[]{ "RequestStatus" } },
			   };
			string SelectQuery = "Request_Data_ID";
			var conls = cols.Split(',').Distinct();
			foreach (var i in conls)
			{
				var Table = ReturnedXols.FirstOrDefault(q => q.Cols.Contains(i));
				if (Table != null)
				{
					SelectQuery += ",";
					if (Table.Table.Contains("Personel_Data.") || Table.Table.Equals("Service_Type") || Table.Table.Equals("Request_Type") || Table.Table.Equals("Request_State"))
						SelectQuery += SelectQuery.Contains(Table.Table) ? "" : Table.Table;
					else if (Table.Table == "")
						SelectQuery += i;
					else
						SelectQuery += Table.Table + "." + i;
					if (i.Contains("City"))
						SelectQuery += ",Personel_Data.Address_City";
					if (i.Contains("Region"))
						SelectQuery += ",Personel_Data.Adress_Region";
				}
			}
			return $"new({SelectQuery})";
		}
		[HttpPost]
		public async Task<IHttpActionResult> ReportRequests(int? ST, int? RT, int? MT, int? location, int? Unit, int? ReqStatus, bool? ReqSource, DateTime? DF, DateTime? DT, string Columns)
		{
			try
			{
				var Pred = PredicateBuilder.New<Request_Data>(q => q.Is_Archived);
				if (ST.HasValue)
					Pred.And(q => q.Service_Type_ID == ST);
				if (RT.HasValue)
					Pred.And(q => q.Request_Type_ID == RT);
				if (MT.HasValue)
					if (MT == 0)
						Pred.And(q => q.Personel_Data.IAU_ID_Number == "" || q.Personel_Data.IAU_ID_Number != null);
					else
						Pred.And(q => q.Personel_Data.IAU_ID_Number != "" && q.Personel_Data.IAU_ID_Number != null);
				if (DF.HasValue)
					Pred.And(q => q.CreatedDate >= DF);
				if (DT.HasValue)
					Pred.And(q => q.CreatedDate <= DT);
				if (location.HasValue)
					Pred.And(q => q.RequestTransaction.Count(s => s.Units.Units_Location_ID == location) != 0);
				if (Unit.HasValue)
					Pred.And(q => q.RequestTransaction.Count(s => s.ToUnitID == Unit) != 0);
				if (ReqStatus.HasValue)
					Pred.And(q => q.Request_State_ID == ReqStatus);
				if (ReqSource.HasValue)
					Pred.And(q => q.IsTwasul_OC == ReqSource);

				var data = p.Request_Data.Where(Pred).Select(SelectQueryData(Columns)).Distinct();
				return Ok(new ResponseClass() { success = true, result = data });

			}
			catch (Exception ee)
			{
				return Ok(new ResponseClass() { success = false, result = ee });
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
				p.Dispose();
			base.Dispose(disposing);
		}
	}
}