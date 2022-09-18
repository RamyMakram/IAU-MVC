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
using System.Threading;

namespace IAUBackEnd.Admin.Controllers
{
    public class RequestController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        // GET: api/Request
        public async Task<IHttpActionResult> GetRequests_Data(int UserID)
        {
            try
            {
                var User = db.Users.Include(q => q.Units).Include(q => q.Job).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted);
                if (User == null)
                    return Ok(new ResponseClass() { success = false, result = "Null Us" });
                var ComplaintID = await db.Request_Type.FirstOrDefaultAsync(s => s.Request_Type_Name_EN.ToLower().StartsWith("comp"));
                var Unit = User.Units;

                if (Unit.IS_Mostafid)
                {
                    var data = db.Request_Data.Where(q =>
                    (User.Job.IsModear ? true : (q.Request_Type_ID != ComplaintID.Request_Type_ID/*Dont Get Complaint Request*/)) &&/*Check if is Modear and mostafid return shakway Orders */
                    !q.Is_Archived && q.Request_State_ID != 5 && (q.RequestTransaction.Count() == 0 || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment != null)).Select(q => new { Required_Fields_Notes = q.RequestTransaction.Count() == 0 ? q.Required_Fields_Notes : q.RequestTransaction.OrderByDescending(s => s.CommentDate).FirstOrDefault().Comment, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, CreatedDate = q.RequestTransaction.Count == 0 ? q.CreatedDate : q.RequestTransaction.OrderByDescending(ssd => ssd.ID).FirstOrDefault().CommentDate, Readed = q.Readed ?? false, q.Request_State_ID, }).OrderByDescending(q => q.Request_Data_ID);
                    var ss = data.ToList();
                    return Ok(new ResponseClass() { success = true, result = data });
                }
                else
                {
                    var data = db.RequestTransaction.Where(w => !w.Request_Data.Is_Archived && (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID && w.Request_Data.Request_State_ID != 5).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, CreatedDate = q.ForwardDate, q.Readed }).OrderByDescending(q => q.CreatedDate);
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
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Get", Oldval: null, Newval: null, es: out _, syslog: out _, ID: null, notes: "Filter Requests");
                if (logstate)
                    await db.SaveChangesAsync();
                else
                    throw new Exception();

                var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
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

                    var data = db.Request_Data.Where(Pred).Select(q => new { Required_Fields_Notes = q.RequestTransaction.Count() == 0 ? q.Required_Fields_Notes : q.RequestTransaction.OrderByDescending(s => s.CommentDate).FirstOrDefault().Comment, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, CreatedDate = q.RequestTransaction.Count == 0 ? q.CreatedDate : q.RequestTransaction.OrderByDescending(ss => ss.ID).FirstOrDefault().CommentDate, Readed = q.Readed ?? false, q.Request_State_ID, }).OrderByDescending(q => q.Request_Data_ID);
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

                    var data = db.RequestTransaction.Where(Pred).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, CreatedDate = q.ForwardDate, q.Readed }).OrderByDescending(q => q.CreatedDate);
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
            var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
            if (Unit.IS_Mostafid)
            {
                var data = db.Request_Data.Where(q => !q.Is_Archived && q.Request_State_ID != 5 && q.RequestTransaction.Count() != 0 && (q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment == "" || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment == null)).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
                return Ok(new ResponseClass() { success = true, result = data });
            }
            else
            {
                var data = db.RequestTransaction.Where(w => !w.Request_Data.Is_Archived && (w.Comment != "" && w.Comment != null) && w.ToUnitID == Unit.Units_ID && w.Request_Data.Request_State_ID != 5).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, q.Request_Data.CreatedDate, q.Request_Data.Readed }).Distinct().OrderByDescending(q => q.CreatedDate);
                return Ok(new ResponseClass() { success = true, result = data });
            }
        }
        public async Task<IHttpActionResult> GetFilterSendedRequests_Data(int? ST, int? RT, int? MT, DateTime? DF, DateTime? DT, int UserID)
        {
            try
            {
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Get", Oldval: null, Newval: null, es: out _, syslog: out _, ID: null, notes: "Filter SendedRequests");
                if (logstate)
                    await db.SaveChangesAsync();
                else
                    throw new Exception();
                var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
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

                    var data = db.Request_Data.Where(Pred).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
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

                    var data = db.RequestTransaction.Where(Pred).Select(q => new { q.Request_Data.Required_Fields_Notes, q.Request_Data.Request_Data_ID, q.Request_Data.Service_Type, q.Request_Data.Request_Type, q.Request_Data.Personel_Data, q.Request_Data.CreatedDate, q.Request_Data.Readed }).Distinct().OrderByDescending(q => q.CreatedDate);
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
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Get", Oldval: null, Newval: null, es: out _, syslog: out _, ID: null, notes: "Filter ArchivedRequests");
            if (logstate)
                await db.SaveChangesAsync();
            else
                return Ok(new ResponseClass() { success = false });

            var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
            if (Unit.IS_Mostafid)
            {
                var data = db.Request_Data.Where(q => q.Is_Archived).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
                return Ok(new ResponseClass() { success = true, result = data });
            }
            return Ok(new ResponseClass() { success = false });
        }

        public async Task<IHttpActionResult> GetFilterArchivedRequests_Data(int? ST, int? RT, int? MT, DateTime? DF, DateTime? DT, int UserID)
        {
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Get", Oldval: null, Newval: null, es: out _, syslog: out _, ID: null, notes: "Filter ArchivedRequests");
            if (logstate)
                await db.SaveChangesAsync();
            else
                return Ok(new ResponseClass() { success = false });


            var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
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

                var data = db.Request_Data.Where(Pred).Select(q => new { q.Required_Fields_Notes, q.Request_Data_ID, q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.Readed, q.Request_State_ID, }).Distinct().OrderByDescending(q => q.Request_Data_ID);
                return Ok(new ResponseClass() { success = true, result = data });
            }
            return Ok(new ResponseClass() { success = false });
        }

        public async Task<IHttpActionResult> GetRequestsCount(int UserID)
        {
            var User = db.Users.Include(q => q.Units).Include(q => q.Job).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted);
            if (User == null)
                return Ok(new ResponseClass() { success = false, result = "Null Us" });
            var ComplaintID = await db.Request_Type.FirstOrDefaultAsync(s => s.Request_Type_Name_EN.ToLower().StartsWith("comp"));
            var Unit = User.Units; if (Unit.IS_Mostafid)
                return Ok(new ResponseClass()
                {
                    success = true,
                    result = db.Request_Data.Count(q =>
                            (User.Job.IsModear ? true : (q.Request_Type_ID != ComplaintID.Request_Type_ID/*Dont Get Complaint Request*/)) &&/*Check if is Modear and mostafid return shakway Orders */
                            !q.Is_Archived && q.Request_State_ID != 5 && (q.RequestTransaction.Count() == 0 || q.RequestTransaction.OrderByDescending(s => s.ID).FirstOrDefault().Comment != null))
                });
            else
                return Ok(new ResponseClass()
                {
                    success = true,
                    result = db.RequestTransaction.Count(w =>
                        (w.Request_Data.Request_Type_ID != ComplaintID.Request_Type_ID/*Dont Get Complaint Request*/) &&
                        !w.Request_Data.Is_Archived && (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID && !w.Readed)
                });
        }

        //[EnableCors(origins: "*", headers: "*", methods: "*")]

        public async Task<IHttpActionResult> GetRequest_Data(int id, int UserID)
        {
            var trans = db.Database.BeginTransaction();
            try
            {
                var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted)?.Units;
                if (Unit == null)
                    return Ok(new ResponseClass() { success = false });

                Request_Data request_Data;

                if (Unit.IS_Mostafid)
                    request_Data = db.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Person_Eform).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Units).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.Request_State_ID != 5 && q.Request_Data_ID == id && (q.RequestTransaction.Count == 0 || q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Comment != null));
                else
                    request_Data = db.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Person_Eform).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Units).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents))
                        .FirstOrDefault(q => q.Request_Data_ID == id && ((q.RequestTransaction.Count == 0 || q.RequestTransaction.Count(w => (w.Comment == "" || w.Comment == null) && w.ToUnitID == Unit.Units_ID) != 0)));

                if (request_Data == null)
                    return Ok(new ResponseClass() { success = false });

                var OldVals = JsonConvert.SerializeObject(request_Data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                if (Unit.IS_Mostafid)
                {
                    if (request_Data.Readed == null || !request_Data.Readed.Value)
                    {
                        request_Data.Readed = true;
                        request_Data.ReadedDate = Helper.GetDate();
                        db.SaveChanges();
                    }
                }
                else if (!Unit.IS_Mostafid)
                {
                    var data = request_Data.RequestTransaction.ToList().Last();
                    data.Readed = true;
                    if (request_Data.Request_State_ID == 2)
                        request_Data.Request_State_ID = 3;
                    db.SaveChanges();
                }
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: request_Data, es: out _, syslog: out _, ID: request_Data.Request_Data_ID, notes: "Read Request");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true,
                        result = new { request_Data, request_Data.Units.Units_Location_ID, request_Data.Units.Building_Number }
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
        public async Task<IHttpActionResult> GetSendedRequest_Data(int id, int UserID)
        {
            try
            {
                var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
                Request_Data request_Data;
                if (Unit.IS_Mostafid)
                    request_Data = db.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Person_Eform).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).FirstOrDefault(q => q.Request_State_ID != 5 && q.Request_Data_ID == id && (q.RequestTransaction.Count == 0 || q.RequestTransaction.OrderByDescending(w => w.ID).FirstOrDefault().Comment == null));
                else
                    request_Data = db.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Person_Eform).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents))
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
                var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted).Units;
                if (Unit.IS_Mostafid)
                {
                    Request_Data request_Data = db.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data.ID_Document1).Include(q => q.Personel_Data.Country1).Include(q => q.Personel_Data.City).Include(q => q.Personel_Data.Region).Include(q => q.Personel_Data.Country2).Include(q => q.Personel_Data.Applicant_Type).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).Include(q => q.Request_File.Select(w => w.Required_Documents)).Include(q => q.Units).FirstOrDefault(q => q.Is_Archived && q.Request_Data_ID == id);
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
            var RequestTransaction = db.RequestTransaction.Include(q => q.Units1).Where(q => q.Request_ID == id);
            return Ok(new ResponseClass() { success = true, result = RequestTransaction });
        }


        [HttpPost]
        public async Task<IHttpActionResult> SaveApplicantData()
        {
            DbContextTransaction transaction = db.Database.BeginTransaction();
            try
            {
                Request_Data request_Data = new Request_Data();
                var provider = new MultipartMemoryStreamProvider();
                await Request.Content.ReadAsMultipartAsync(provider);
                var buffer = await provider.Contents.Last().ReadAsStringAsync();
                request_Data = JsonConvert.DeserializeObject<Request_Data>(buffer);

                var model = request_Data.Personel_Data;
                buffer = await provider.Contents[provider.Contents.Count - 2].ReadAsStringAsync();
                var E_Forms_Answer = JsonConvert.DeserializeObject<List<Models.E_Forms_Answer>>(buffer);
                Personel_Data personel_Data = db.Personel_Data.FirstOrDefault(q => (q.ID_Document == model.ID_Document && q.ID_Number == model.ID_Number) || q.Mobile == model.Mobile);
                if (personel_Data == null)
                {
                    db.Personel_Data.Add(model);
                    await db.SaveChangesAsync();
                    request_Data.Personel_Data_ID = model.Personel_Data_ID;
                }
                else
                    request_Data.Personel_Data_ID = personel_Data.Personel_Data_ID;
                request_Data.CreatedDate = Helper.GetDate();

                #region CheckDeleted
                if (db.Request_Type.Find(request_Data.Request_Type_ID).Deleted)
                {
                    transaction.Rollback();
                    return Ok(new
                    {
                        result = "Del RT",
                        success = false
                    });
                }

                if (request_Data.Sub_Services_ID != null && db.Sub_Services.Find(request_Data.Sub_Services_ID).Deleted)
                {
                    transaction.Rollback();
                    return Ok(new
                    {
                        result = "Del SS",
                        success = false
                    });
                }

                if (db.Service_Type.Find(request_Data.Service_Type_ID).Deleted)
                {
                    transaction.Rollback();
                    return Ok(new
                    {
                        result = "Del ST",
                        success = false
                    });
                }
                #endregion

                //request_Data.Request_State_ID = 1;
                request_Data.IsTwasul_OC = false;
                request_Data.Readed = false;
                request_Data.Is_Archived = false;
                request_Data.Request_State_ID = 1;
                db.Request_Data.Add(request_Data);
                await db.SaveChangesAsync();
                var ISInquiry = db.Request_Type.Any(q => q.Request_Type_ID == request_Data.Request_Type_ID && q.IsRequestType);
                if (ISInquiry)
                {
                    var Eforms = db.E_Forms.Include(q => q.Question).Include(q => q.Question.Select(s => s.Separator)).Include(q => q.Question.Select(s => s.Paragraph)).Include(q => q.UnitToApprove).Where(q => q.IS_Action && q.SubServiceID == request_Data.Sub_Services_ID && !q.Deleted).Select(q => new { q.Code, q.Name, q.Name_EN, Question = q.Question, Eform_Approval = q.Units });
                    foreach (var eform in Eforms)
                    {
                        var Eform_Person = new Person_Eform { Code = eform.Code, Name = eform.Name, Name_EN = eform.Name_EN, RequestID = request_Data.Request_Data_ID, Person_ID = request_Data.Personel_Data_ID, FillDate = request_Data.CreatedDate.Value };
                        foreach (var i in eform.Question)
                        {
                            var Inser_Qty = E_Forms_Answer.FirstOrDefault(q => q.Question_ID == i.ID);
                            if (i.Type == "S")
                            {
                                Inser_Qty = new Models.E_Forms_Answer();
                                Inser_Qty.Name = "S" + (i.Separator.IsEmpty ? "" : "L");
                                Inser_Qty.Name_En = Inser_Qty.Name;
                                Inser_Qty.FillDate = Helper.GetDate();
                                Inser_Qty.Type = i.Type;
                                Inser_Qty.Value = "";
                                Inser_Qty.Value_En = "";
                                Inser_Qty.Index_Order = i.Index_Order;
                                Eform_Person.E_Forms_Answer.Add(Inser_Qty);
                            }
                            else if (i.Type == "P")
                            {
                                Inser_Qty = new Models.E_Forms_Answer();
                                Inser_Qty.Name = i.Paragraph.Name;
                                Inser_Qty.Name_En = i.Paragraph.Name;
                                Inser_Qty.FillDate = Helper.GetDate();
                                Inser_Qty.Type = i.Type;
                                Inser_Qty.Value = "";
                                Inser_Qty.Value_En = "";
                                Inser_Qty.Index_Order = i.Index_Order;
                                Eform_Person.E_Forms_Answer.Add(Inser_Qty);
                            }
                            else if (i.Type == "T")
                            {
                                Inser_Qty = new Models.E_Forms_Answer();
                                Inser_Qty.Name = i.LableName;
                                Inser_Qty.Name_En = i.LableName_EN;
                                Inser_Qty.FillDate = Helper.GetDate();
                                Inser_Qty.Value = "";
                                Inser_Qty.Value_En = "";
                                Inser_Qty.Type = i.Type;
                                Inser_Qty.Index_Order = i.Index_Order;
                                Eform_Person.E_Forms_Answer.Add(Inser_Qty);
                            }
                            else if (i.Type == "G")
                            {
                                //Inser_Qty = new Models.E_Forms_Answer();
                                Inser_Qty.Name = i.LableName;
                                Inser_Qty.Name_En = i.LableName_EN;
                                Inser_Qty.FillDate = Helper.GetDate();
                                Inser_Qty.Value = "";
                                Inser_Qty.Value_En = "";
                                Inser_Qty.Type = i.Type;
                                Inser_Qty.Index_Order = i.Index_Order;
                                Eform_Person.E_Forms_Answer.Add(Inser_Qty);
                            }
                            else if (Inser_Qty != null || (Inser_Qty == null && !i.Requird))
                            {
                                Inser_Qty.Name = i.LableName;
                                Inser_Qty.Name_En = i.LableName_EN;
                                Inser_Qty.FillDate = Helper.GetDate();
                                Inser_Qty.Type = i.Type;
                                Inser_Qty.Index_Order = i.Index_Order;
                                Eform_Person.E_Forms_Answer.Add(Inser_Qty);
                            }
                            else
                                throw new Exception("Ansqares");
                        }
                        Eform_Person.Preview_EformApproval.Add(new Preview_EformApproval { OwnEform = true, Name = eform.Eform_Approval.Units_Name_AR, Name_En = eform.Eform_Approval.Units_Name_EN, UnitID = eform.Eform_Approval.Units_ID });

                        db.Person_Eform.Add(Eform_Person);
                    }
                }
                await db.SaveChangesAsync();
                var path = HttpContext.Current.Server.MapPath("~");
                var requestpath = Path.Combine("RequestFiles", request_Data.Request_Data_ID.ToString());
                Directory.CreateDirectory(Path.Combine(path, requestpath));
                if (provider.Contents.Count > 2)
                {
                    var count = 0;
                    if (db.Request_Type.Any(q => q.Request_Type_ID == request_Data.Request_Type_ID && q.IsRequestType))
                    {
                        var RequiredFiles = db.Required_Documents.Where(q => q.SubServiceID == request_Data.Sub_Services_ID && !q.Deleted).ToList();
                        foreach (var i in RequiredFiles)
                        {
                            var file = provider.Contents.FirstOrDefault(q => q.Headers.ContentDisposition.FileName != null && q.Headers.ContentDisposition.FileName.StartsWith("" + i.ID));
                            if (file == null)
                            {
                                transaction.Rollback();
                                return Ok(new
                                {
                                    result = "Error In Suppoerted Files",
                                    success = false
                                });
                            }
                            var ReqFile = file.Headers.ContentDisposition.FileName.Split('|');

                            var ReqFileID = int.Parse(ReqFile[0]);

                            var filename = ReqFile[1].Trim('\"');
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
                    int length = provider.Contents.Count - 2;
                    for (; count < length; count++)
                    {
                        var file = provider.Contents[count];
                        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                        var Strambuffer = await file.ReadAsByteArrayAsync();
                        var filepath = Path.Combine(requestpath, filename);
                        File.WriteAllBytes(Path.Combine(path, filepath), Strambuffer);
                        db.Request_File.Add(new Request_File()
                        {
                            Request_ID = request_Data.Request_Data_ID.Value,
                            CreatedDate = DateTime.Now,
                            File_Name = filename,
                            File_Path = filepath.Replace("\\", "/")
                        });
                    }

                }

                db.SaveChanges();
                transaction.Commit();

                #region WebSocket

                var sendeddata = db.Request_Data.Where(q => q.Request_Data_ID == request_Data.Request_Data_ID).Select(q => new { q.Service_Type, q.Request_Type, q.Personel_Data, q.CreatedDate, q.Request_Data_ID, q.Required_Fields_Notes }).First();
                var ISComplaint = (sendeddata?.Request_Type?.Request_Type_Name_EN ?? "").ToLower().StartsWith("comp");//Modear of mostfaid unit
                var Users = db.Users.Where(q => q.Units.IS_Mostafid && !q.Deleted && (ISComplaint ? q.Job.IsModear : true)).Select(q => q.User_ID).ToArray();

                string message = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                WebSocketManager.SendToMulti(Users, message);

                #endregion


                new Thread(() =>
                {
                    _ = NotifyUser(model.Mobile, model.Email, @"عزيزي المستفيد ، تم استلام طلبكم بنجاح ، وسيتم افادتكم بالكود الخاص بالطلب خلال 3 ساعات", @"Dear Mostafid, your order has been successfully received, and you will be notified of the order code within 3 hours");
                }).Start();
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
                    result = e,
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

                    string url = $"http://basic.unifonic.com/wrapper/sendSMS.php?appsid=su7G9tOZc6U0kPVnoeiJGHUDMKe8tp&msg={message_ar}&to={Mobile}&sender=IAU-BSC&baseEncode=False&encoding=UCS2";
                    h.BaseAddress = new Uri(url);

                    var res = h.GetAsync("").Result.Content.ReadAsStringAsync().Result;
                }
                if (Email != null)
                {
                    var message = $@"
					<p dir='ltr'>{message_en}</p>
					<p dir='rtl'>{message_ar}</p>
					";
                    SmtpClient smtpClient = new SmtpClient("10.30.1.101", 25);

                    smtpClient.Credentials = new System.Net.NetworkCredential("noreply.bsc@iau.edu.sa", "Bsc@33322");
                    // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //smtpClient.EnableSsl = true;
                    MailMessage mail = new MailMessage();

                    //Setting From , To and CC
                    mail.From = new MailAddress("noreply.bsc@iau.edu.sa", "Mustafid");
                    mail.To.Add(new MailAddress(Email));
                    mail.Subject = "IAU Notify";
                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    smtpClient.Send(mail);
                }

                //SmtpClient smtpClient = new SmtpClient("mail.iau-bsc.com", 25);

                //smtpClient.Credentials = new System.Net.NetworkCredential("ramy@iau-bsc.com", "ENGGGGAAA1448847@");
                //// smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                ////smtpClient.EnableSsl = true;
                //MailMessage mail = new MailMessage();

                ////Setting From , To and CC
                //mail.From = new MailAddress("ramy@iau-bsc.com", "Mustafid");
                //mail.To.Add(new MailAddress(Email));
                //mail.Subject = "IAU Notify";
                //mail.Body = message;
                //mail.IsBodyHtml = true;
                //smtpClient.Send(mail);
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

            var req = db.Request_Data.Include(q => q.Personel_Data).FirstOrDefault(q => q.Request_Data_ID == RequestIID);
            if (req.TempCode == "" || req.TempCode == null)
            {
                var trans = db.Database.BeginTransaction();

                var OldVals = JsonConvert.SerializeObject(req, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


                req.IsTwasul_OC = IsTwasul_OC;
                if (db.Service_Type.Find(Service_Type_ID).Deleted)
                    return Ok(new ResponseClass() { success = false, result = "Del ST" });

                req.Service_Type_ID = Service_Type_ID;

                if (db.Request_Type.Find(Request_Type_ID).Deleted)
                    return Ok(new ResponseClass() { success = false, result = "Del RT" });
                req.Request_Type_ID = Request_Type_ID;

                if (db.Units.Find(Unit_ID).Deleted)
                    return Ok(new ResponseClass() { success = false, result = "Del U" });

                req.Unit_ID = Unit_ID;

                string Code = GetCode(RequestIID, IsTwasul_OC, Service_Type_ID, Request_Type_ID, locations, BuildingSelect, Unit_ID, type);
                if (req.Code_Generate == "" || req.Code_Generate == null)//Check If Code not Genrated
                {
                    req.Code_Generate = Code;
                    req.TempCode = Code;
                    req.GenratedDate = Helper.GetDate();
                    db.SaveChanges();
                    var ssss = $@"عزيزي المستفيد، نفيدكم علما بأن كود الطلب الخاص بكم هو: '{Code}' برجاء اسخدامه في حالة الإستعلام";
                    var ssss_EN = $@"Dear Mostafid, we inform you that your request code is: '{Code}' Please use it in case of query";
                    db.PhoneNumberNotification.Add(new PhoneNumberNotification { Message = ssss, Message_EN = ssss_EN, NotiDate = DateTime.Now, PhoneNumber = req.Personel_Data.Mobile, RequestID = RequestIID, UserID = req.Personel_Data_ID });
                    new Thread(() =>
                    {
                        _ = NotifyUser(req.Personel_Data.Mobile, req.Personel_Data.Email, ssss, ssss_EN);
                    }).Start();

                }
                else//Check If Code Genrated
                {
                    req.TempCode = Code;
                    req.GenratedDate = Helper.GetDate();
                }

                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: req, es: out _, syslog: out _, ID: req.Request_Data_ID, notes: "Coding Request");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            else
                return Ok(new ResponseClass() { success = false });

        }

        [HttpPost]
        public async Task<IHttpActionResult> Forward(int RequestIID, int Unit_ID, Nullable<DateTime> Expected, [FromBody] string comment)
        {
            var trans = db.Database.BeginTransaction();
            try
            {
                Request_Data sendeddata = db.Request_Data.Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == RequestIID);
                if (sendeddata.TempCode != "")
                {
                    var OldVals = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    var willsend = JsonConvert.DeserializeObject<Request_Data>(OldVals);


                    db.RequestTransaction.Add(new RequestTransaction() { Request_ID = RequestIID, ExpireDays = Expected, ForwardDate = Helper.GetDate(), ToUnitID = Unit_ID, Readed = false, FromUnitID = db.Units.First(q => q.IS_Mostafid).Units_ID, Code = sendeddata.TempCode, MostafidComment = comment, Is_Reminder = false });
                    sendeddata.TempCode = "";
                    willsend.TempCode = "";
                    if (sendeddata.Request_State_ID == 1)
                    {
                        sendeddata.Request_State_ID = 2;
                        willsend.Request_State_ID = 2;
                    }
                    db.SaveChanges();

                    willsend.Readed = false;
                    var Users = db.Users.Where(q => q.Units.Units_ID == Unit_ID && !q.Deleted).Select(q => q.User_ID).ToArray();

                    willsend.Required_Fields_Notes = comment.Trim().Length == 0 ? willsend.Required_Fields_Notes : comment;

                    string message = JsonConvert.SerializeObject(willsend, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                    WebSocketManager.SendToMulti(Users, message);

                    var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: willsend, es: out _, syslog: out _, ID: willsend.Request_Data_ID, notes: "Forward Request");
                    if (logstate)
                    {
                        await db.SaveChangesAsync();
                        trans.Commit();
                        return Ok(new ResponseClass()
                        {
                            success = true
                        });
                    }
                    else
                    {
                        trans.Rollback();
                        return Ok(new ResponseClass() { success = false });
                    }
                }
                else
                    return Ok(new ResponseClass() { success = false });

            }
            catch (Exception ee)
            {
                trans.Rollback();
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
            var CurrentUnit = db.Users.FirstOrDefault(q => q.User_ID == UserID && !q.Deleted)?.UnitID;
            if (CurrentUnit == null)
                return Ok(new ResponseClass() { success = false });

            Request_Data sendeddata = db.Request_Data.Include(q => q.RequestTransaction).Include(q => q.Request_File).Include(q => q.Personel_Data.Country).Include(q => q.Personel_Data).Include(q => q.Service_Type).Include(q => q.Request_Type).FirstOrDefault(q => q.Request_Data_ID == RequestID);
            var trans = sendeddata.RequestTransaction.Last();
            if (CurrentUnit == trans.ToUnitID)
            {
                var transa = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                trans.Comment = Comment;
                trans.CommentDate = Helper.GetDate();
                trans.CommentType = CommentType;
                if (sendeddata.Request_State_ID == 3)
                    sendeddata.Request_State_ID = 4;
                sendeddata.Readed = false;
                db.SaveChanges();
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: sendeddata, es: out _, syslog: out _, ID: sendeddata.Request_Data_ID, notes: "Reply to Request");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    transa.Commit();
                }
                else
                {
                    transa.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }

                sendeddata.Required_Fields_Notes = Comment;

                var ISComplaint = (sendeddata?.Request_Type?.Request_Type_Name_EN ?? "").ToLower().StartsWith("comp");//Modear of mostfaid unit


                var Users = db.Users.Where(q => q.Units.IS_Mostafid && !q.Deleted && (ISComplaint ? q.Job.IsModear : true)).Select(q => q.User_ID).ToArray();
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
            var IsMostafid = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted)?.Units;
            Request_Data sendeddata = db.Request_Data.FirstOrDefault(q => q.Request_Data_ID == RequestID);

            if (IsMostafid == null || sendeddata == null)
                return Ok(new ResponseClass() { success = false });

            var trans = db.RequestTransaction.Where(q => q.Request_ID == RequestID).OrderByDescending(q => q.ID).FirstOrDefault();
            if (IsMostafid.IS_Mostafid)
            {
                var transa = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                trans.Comment = "Delayed";
                db.RequestTransaction.Add(new RequestTransaction() { Request_ID = RequestID, ForwardDate = Helper.GetDate(), ToUnitID = trans.ToUnitID, Readed = false, FromUnitID = IsMostafid.Units_ID, Code = trans.Code, MostafidComment = Comment, Is_Reminder = true });
                sendeddata.Readed = false;
                db.SaveChanges();
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: sendeddata, es: out _, syslog: out _, ID: sendeddata.Request_Data_ID, notes: "Add Mostafid Reminder");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    transa.Commit();
                }
                else
                {
                    transa.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
                var Users = db.Users.Where(q => q.Units.Units_ID == trans.ToUnitID && !q.Deleted).Select(q => q.User_ID).ToArray();
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
            var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted)?.Units;
            if (Unit == null)
                return Ok(new ResponseClass() { success = false });

            if (Unit.IS_Mostafid)
            {
                var trans = db.Database.BeginTransaction();

                var hasNoErrorLog = true;
                foreach (var i in requests)
                {
                    Request_Data sendeddata = db.Request_Data.FirstOrDefault(q => q.Request_Data_ID == i);
                    if (sendeddata != null && sendeddata.Request_State_ID == 1)
                    {
                        var OldVals = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                        sendeddata.Request_State_ID = 5;
                        sendeddata.Is_Archived = true;
                        var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: sendeddata, es: out _, syslog: out _, ID: sendeddata.Request_Data_ID, notes: "Archived Request");
                        hasNoErrorLog &= logstate;
                    }
                }
                if (hasNoErrorLog)
                {
                    db.SaveChanges();
                    trans.Commit();
                    return Ok(new ResponseClass() { success = true });
                }
                trans.Rollback();
            }
            return Ok(new ResponseClass() { success = false });
        }

        [HttpPost]
        public async Task<IHttpActionResult> ApproveEform(int UnitID, int EformID)
        {
            var Unit = await db.Units.Include(q => q.Unit_Signature).FirstOrDefaultAsync(q => q.Units_ID == UnitID && !q.Deleted);
            if (Unit?.Unit_Signature == null)
                return Ok(new ResponseClass() { success = false, result = "Signature" });

            var approval = await db.Preview_EformApproval.FirstOrDefaultAsync(q => q.PersonEform == EformID && q.OwnEform);
            if (approval == null)
                return Ok(new ResponseClass() { success = false, result = "NotExist" });

            if (Unit.Units_ID == approval.UnitID && approval.SignDate == null)
            {
                var trans = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(approval, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                approval.SignDate = Helper.GetDate();

                db.SaveChanges();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.EformApproval, Method: "Update", Oldval: OldVals, Newval: approval, es: out _, syslog: out _, ID: approval.Person_Eform.RequestID, notes: "Unit Sign Efrom Request");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass()
                    {
                        success = true
                    });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            return Ok(new ResponseClass() { success = false, result = "Already" });
        }

        [HttpPost]
        public async Task<IHttpActionResult> CloseRequest(int UserID, int RequestID)
        {
            Request_Data sendeddata = db.Request_Data.Include(q => q.RequestTransaction.Select(s => s.Units1)).Include(q => q.Personel_Data).FirstOrDefault(q => q.Request_Data_ID == RequestID);
            var Unit = db.Users.Include(q => q.Units).FirstOrDefault(q => q.User_ID == UserID && !q.Deleted)?.Units;
            if (sendeddata == null || Unit == null)
                return Ok(new ResponseClass() { success = false });

            if (Unit.IS_Mostafid && (sendeddata.Request_State_ID != 2))
            {
                var trans = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(sendeddata, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                sendeddata.Request_State_ID = 5;
                sendeddata.Is_Archived = true;
                string ssss = $@"عزيزي المستفيد، تم الانتهاء من الطلب رقم '{sendeddata.Code_Generate}'.", ssss_EN = $"Dear Mostafid, Request number '{sendeddata.Code_Generate}' has been completed";

                db.PhoneNumberNotification.Add(new PhoneNumberNotification { Message = ssss, Message_EN = ssss_EN, NotiDate = DateTime.Now, PhoneNumber = sendeddata.Personel_Data.Mobile, RequestID = sendeddata.Request_Data_ID, UserID = sendeddata.Personel_Data_ID });

                db.SaveChanges();
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Update", Oldval: OldVals, Newval: sendeddata, es: out _, syslog: out _, ID: sendeddata.Request_Data_ID, notes: "Close Request");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }


                if (sendeddata.RequestTransaction.Count != 0)
                {
                    string td_data = "";
                    var req_trans = sendeddata.RequestTransaction.Where(s => s.CommentDate.HasValue).OrderByDescending(q => q.CommentDate);
                    foreach (var i in req_trans)
                    {
                        td_data += $@"
                                <tr>
                                    <td><p>{i.Units1.Units_Name_AR}<p/> </br> <p>{i.Units1.Units_Name_EN}<p/> </td>
                                    <td>{i.Comment}</td>
                                    <td>{i.CommentDate?.ToString("dd-MM-yyyy HH:mm") ?? ""}</td>
                                <tr>";
                    }
                    string tableStyle = @"
                            <style>
                            </style>";
                    string message = $@"
                                {tableStyle}
                                    <table dir='rtl' border='1' cellpadding='1' cellspacing='1' width='100%'>
                                        <thead>
                                            <tr>
                                                <th><p> اسم الفئة الإدارية </p><p> Unit Name</p></th>
                                                <th><p>التعليق </p><p> Comment</p></th>
                                                <th><p>التاريخ </p><p> Date</p></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {td_data}
                                        </tbody>
                                    <table>";

                    new Thread(() =>
                    {
                        _ = NotifyUser(sendeddata.Personel_Data.Mobile, sendeddata.Personel_Data.Email, ssss + (req_trans.Count() == 0 ? "" : message), ssss_EN);
                    }).Start();
                }
                return Ok(new ResponseClass() { success = true });
            }
            return Ok(new ResponseClass() { success = false });
        }

        private string GetCode(int RequestIID, bool IsTwasul_OC, int Service_Type_ID, int Request_Type_ID, int? locations, string BuildingSelect, int Unit_ID, string type)
        {
            var unit = db.Units.Include(q => q.Units_Location).FirstOrDefault(q => q.Units_ID == Unit_ID);
            int? loc = (locations ?? unit.Units_Location.Location_ID);
            var Location = db.Units_Location.FirstOrDefault(q => q.Location_ID == loc).Code;
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
                var data = db.Request_Data.Include(q => q.Units).Include(q => q.Request_State).Where(q => q.Code_Generate == Code)
                    .Select(q => new { q.IsTwasul_OC, q.Request_State, q.Request_Data_ID, q.Request_State_ID }).FirstOrDefault();
                return Ok(new ResponseClass() { success = true, result = new { Request = data, State = db.RequestTransaction.Where(q => q.Request_ID == data.Request_Data_ID).Include(q => q.Units).OrderByDescending(w => w.ID).FirstOrDefault() } });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
        private string SelectQueryData(string cols)
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
        public async Task<IHttpActionResult> ReportRequests(int? ST, int? RT, int? MT, int? location, int? Unit, int? ReqStatus, bool? ReqSource, DateTime? DF, DateTime? DT, string Columns, bool EndedRequest)
        {
            try
            {
                var Pred = PredicateBuilder.New<Request_Data>(q => q.Is_Archived == EndedRequest);
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
                    Pred.And(q => q.RequestTransaction.Any(s => s.Units.Units_Location_ID == location));
                if (Unit.HasValue)
                    Pred.And(q => q.RequestTransaction.Any(s => s.ToUnitID == Unit) || q.Unit_ID == Unit);
                if (ReqStatus.HasValue)
                    Pred.And(q => q.Request_State_ID == ReqStatus);
                if (ReqSource.HasValue)
                    Pred.And(q => q.IsTwasul_OC == ReqSource);

                var data = db.Request_Data.Where(Pred).Select(SelectQueryData(Columns)).Distinct();
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Request, Method: "Get", Oldval: null, Newval: null, es: out _, syslog: out _, ID: null, notes: "Filter And Report Requests");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    return Ok(new ResponseClass()
                    {
                        success = true,
                        result = data
                    });
                }
                else
                    return Ok(new ResponseClass() { success = false });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}