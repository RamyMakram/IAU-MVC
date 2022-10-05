using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using IAUBackEnd.Admin.Models;
using IAUAdmin.DTO.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using System.IO;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin
{
    public class Helper
    {
        public static DateTime GetDate()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time"));
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.ASCII.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            var str = GetHash(inputString);
            foreach (byte b in str)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static void DelayedRequest()
        {
            string filepath = System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\Log.txt";
            DateTime s = Helper.GetDate();
            var MainDB = new TasahelEntities();
            var Servers = MainDB.Domain.Where(q => q.Enabled).ToList();
            foreach (var server in Servers)
            {
                try
                {
                    MostafidDBEntities p = new MostafidDBEntities(server.ConnectionString);
                    var states = p.Request_State.ToList();
                    List<DelayedTransDTO> times = new List<DelayedTransDTO>();
                    foreach (var i in states)
                    {
                        var data = new List<DelayedTransDTO>();
                        if (i.State_ID == 1 || i.State_ID == 2)
                        {
                            data = p.Request_Data
                                .Where(q =>
                                    !q.Is_Archived &&
                                    q.Request_State_ID == i.State_ID &&
                                    EntityFunctions.DiffDays(q.CreatedDate.Value, s) > i.AllowedDelay
                                ).Select(q =>
                                    new DelayedTransDTO()
                                    {
                                        RequestID = q.Request_Data_ID.Value,
                                        UnitID = q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ToUnitID,
                                        Delayed = EntityFunctions.DiffDays(q.CreatedDate.Value, s).Value,
                                        AddedDate = s,
                                        Readed = false,
                                        RequestCode = q.Code_Generate,
                                        RequestStatus = q.Request_State_ID,
                                        TransactionDate = q.CreatedDate.Value
                                    }
                                ).ToList();
                        }
                        else if (i.State_ID == 3)
                        {
                            data = p.Request_Data.Where(q =>
                                !q.Is_Archived &&
                                q.Request_State_ID == i.State_ID &&
                                EntityFunctions.DiffDays((q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value), s) > i.AllowedDelay
                            ).Select(q =>
                                new DelayedTransDTO()
                                {
                                    RequestID = q.Request_Data_ID.Value,
                                    UnitID = q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ToUnitID,
                                    Delayed = EntityFunctions.DiffDays((q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value), s).Value,
                                    AddedDate = s,
                                    Readed = false,
                                    RequestCode = q.Code_Generate,
                                    RequestStatus = q.Request_State_ID,
                                    TransactionDate = q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value
                                }
                            ).ToList();
                        }
                        else if (i.State_ID == 4)
                        {
                            ///////Transaction in Mostafid Mail-Box and not Encoded
                            var LastState = states.Last();
                            data = p.Request_Data.Include(q => q.RequestTransaction)
                                .Where(q =>
                                    !q.Is_Archived &&
                                    q.Request_State_ID == i.State_ID &&
                                    (q.TempCode == null || q.TempCode == "") &&
                                    q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate != null &&
                                    EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate.Value, s) > LastState.AllowedDelay
                                )
                                .Select(q =>
                                    new DelayedTransDTO()
                                    {
                                        RequestID = q.Request_Data_ID.Value,
                                        UnitID = q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ToUnitID,
                                        Delayed = EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate.Value, s).Value,
                                        AddedDate = s,
                                        Readed = false,
                                        RequestCode = q.Code_Generate,
                                        RequestStatus = q.Request_State_ID,
                                        TransactionDate = (q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate ?? q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate).Value
                                    }
                                ).ToList();
                            times.AddRange(data);
                            ///////Transaction in Mostafid Mail-Box and Encoded or forwarded

                            data = p.Request_Data.Include(q => q.RequestTransaction).Where(q => !q.Is_Archived && q.Request_State_ID == i.State_ID && q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate == null && EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value, s) > i.AllowedDelay).Select(q => new DelayedTransDTO() { RequestID = q.Request_Data_ID.Value, Delayed = EntityFunctions.DiffDays(q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate.Value, s).Value, AddedDate = s, Readed = false, RequestCode = q.Code_Generate, RequestStatus = q.Request_State_ID, TransactionDate = (q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().CommentDate ?? q.RequestTransaction.OrderByDescending(f => f.ID).FirstOrDefault().ForwardDate).Value }).ToList();
                        }
                        times.AddRange(data);
                    }
                    foreach (var i in times.Distinct())
                    {
                        var req = p.DelayedTransaction.FirstOrDefault(q => q.RequestID == i.RequestID && q.RequestStatus == i.RequestStatus && q.DelayedOnUnitID == i.UnitID);
                        if (req == null)
                            p.DelayedTransaction.Add(new DelayedTransaction() { DelayedOnUnitID = i.UnitID, AddedDate = i.AddedDate, Delayed = i.Delayed, Readed = false, RequestCode = i.RequestCode, RequestID = i.RequestID, RequestStatus = i.RequestStatus, TransactionDate = i.TransactionDate });
                        else
                        {
                            req.AddedDate = s;
                            req.Readed = false;
                        }
                    }
                    p.SaveChanges();
                }
                catch (Exception e)
                {
                    File.AppendAllText(filepath, $"\n{s.ToString("dd-MM HH:mm:ss")} Exception, Sleep To 5 minutes->" + JsonConvert.SerializeObject(e), Encoding.UTF8);
                    throw e;
                }
            }
        }
    }
}