using IAUAdmin.DTO.Entity;
using IAUBackEnd.Admin.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Text;
using System.Web.Hosting;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IAUBackEnd.Admin
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
			config.Formatters.Remove(config.Formatters.XmlFormatter);

			config.EnableCors();
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{action}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);
			HostingEnvironment.QueueBackgroundWorkItem(async _ => { await InvokeMethod(); });
		}
		static async Task InvokeMethod()
		{
			while (true)
			{
				string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\Log.txt";
				DateTime s = Helper.GetDate();
				try
				{
					if (s.Hour == 0)
					{
						MostafidDBEntities p = new MostafidDBEntities();
						var states = p.Request_State.ToList();
						List<DelayedTransDTO> times = new List<DelayedTransDTO>();
						foreach (var i in states)
						{
							var data = new List<DelayedTransDTO>();
							if (i.State_ID == 1 || i.State_ID == 2)
							{
								data = p.Request_Data.Where(q => !q.Is_Archived && q.Request_State_ID == i.State_ID && EntityFunctions.DiffDays(q.CreatedDate.Value, s) > i.AllowedDelay).Select(q => new DelayedTransDTO() { RequestID = q.Request_Data_ID.Value, Delayed = EntityFunctions.DiffDays(q.CreatedDate.Value, s).Value, AddedDate = s, Readed = false, RequestCode = q.Code_Generate, RequestStatus = q.Request_State_ID, TransactionDate = q.CreatedDate.Value }).ToList();
							}
							else if (i.State_ID == 3)
							{
								data = p.Request_Data.Where(q => !q.Is_Archived && q.Request_State_ID == i.State_ID && EntityFunctions.DiffDays((q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value), s) > i.AllowedDelay).Select(q => new DelayedTransDTO() { RequestID = q.Request_Data_ID.Value, Delayed = EntityFunctions.DiffDays((q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value), s).Value, AddedDate = s, Readed = false, RequestCode = q.Code_Generate, RequestStatus = q.Request_State_ID, TransactionDate = q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value }).ToList();
							}
							else if (i.State_ID == 4)
							{
								///////Transaction in Mostafid Mail-Box and not Encoded
								var LastState = states.Last();
								data = p.Request_Data.Include(q => q.RequestTransaction).Where(q => !q.Is_Archived && q.Request_State_ID == i.State_ID && (q.TempCode == null || q.TempCode == "") && q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate != null && EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate.Value, s) > LastState.AllowedDelay).Select(q => new DelayedTransDTO() { RequestID = q.Request_Data_ID.Value, Delayed = EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate.Value, s).Value, AddedDate = s, Readed = false, RequestCode = q.Code_Generate, RequestStatus = q.Request_State_ID, TransactionDate = (q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate ?? q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate).Value }).ToList();
								times.AddRange(data);
								///////Transaction in Mostafid Mail-Box and Encoded or forwarded

								data = p.Request_Data.Include(q => q.RequestTransaction).Where(q => !q.Is_Archived && q.Request_State_ID == i.State_ID && q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate == null && EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value, s) > i.AllowedDelay).Select(q => new DelayedTransDTO() { RequestID = q.Request_Data_ID.Value, Delayed = EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value, s).Value, AddedDate = s, Readed = false, RequestCode = q.Code_Generate, RequestStatus = q.Request_State_ID, TransactionDate = (q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate ?? q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate).Value }).ToList();
							}
							times.AddRange(data);
						}
						foreach (var i in times.Distinct())
						{
							var req = p.DelayedTransaction.FirstOrDefault(q => q.RequestID == i.RequestID && q.RequestStatus == i.RequestStatus);
							if (req == null)
								p.DelayedTransaction.Add(new DelayedTransaction() { AddedDate = i.AddedDate, Delayed = i.Delayed, Readed = false, RequestCode = i.RequestCode, RequestID = i.RequestID, RequestStatus = i.RequestStatus, TransactionDate = i.TransactionDate });
							else
							{
								req.AddedDate = s;
								req.Readed = false;
							}
						}
						p.SaveChanges();
						File.AppendAllText(filepath, $"\n{s.ToString("dd-MM HH:mm:ss")} Succses,WithCount:" + times.Count + ",IDs:" + string.Join("|", times.Select(q => q.RequestID).ToArray()), Encoding.UTF8);
						Debug.WriteLine("Succses " + s.ToString("HH:mm:ss"));
						DateTime now = Helper.GetDate();
						DateTime tomorrowMidnight = now.Date.AddDays(1);
						TimeSpan ts = tomorrowMidnight.Subtract(now);
						await Task.Delay((int)Math.Abs(ts.TotalMilliseconds + 1));
					}
					else
					{
						DateTime now = Helper.GetDate();
						DateTime tomorrowMidnight = now.Date.AddDays(1);
						TimeSpan ts = tomorrowMidnight.Subtract(now);
						var xx = $"\n{s.ToString("dd-MM HH:mm:ss")} SleepTo," + ts.ToString();
						File.AppendAllText(filepath, xx, Encoding.UTF8);
						await Task.Delay((int)Math.Abs(ts.TotalMilliseconds + 1));
					}
				}
				catch (Exception e)
				{
					File.AppendAllText(filepath, $"\n{s.ToString("dd-MM HH:mm:ss")} Exception, Sleep To 5 minutes->" + JsonConvert.SerializeObject(e), Encoding.UTF8);
					await Task.Delay(1000 * 60 * 5); // 5 min
				}
			}
		}
	}
}
