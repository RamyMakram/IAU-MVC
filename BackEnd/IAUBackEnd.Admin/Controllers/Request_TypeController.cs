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
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class Request_TypeController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = db.Request_Type.Where(q => q.Deleted) });
        }
        public async Task<IHttpActionResult> GetRequest_Type()
        {
            return Ok(new ResponseClass() { success = true, result = db.Request_Type.Where(q => !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Request_Type.Where(q => q.IS_Action.Value && !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActiveByMainService(int SID)
        {
            return Ok(new ResponseClass() { success = true, result = db.Request_Type.Where(q => q.IS_Action.Value && !q.Deleted && q.Units_Request_Type.Count(w => (w.Units.ServiceTypeID == SID || w.Units.UnitServiceTypes.Count(s => s.ServiceTypeID == SID) != 0) && w.Units.UnitMainServices.Count(r => r.Main_Services.IS_Action.Value && !r.Main_Services.Deleted && r.Main_Services.Sub_Services.Count(g => g.IS_Action.Value && !q.Deleted) != 0) != 0) != 0).Select(q => new { ID = q.Request_Type_ID, Name_AR = q.Request_Type_Name_AR, Name_EN = q.Request_Type_Name_EN, q.Image_Path }) });
        }
        public async Task<IHttpActionResult> GetRequest_Type(int id)
        {
            Request_Type request_Type = await db.Request_Type.FindAsync(id);
            if (request_Type == null || request_Type.Deleted)
                return Ok(new ResponseClass() { success = false, result = request_Type.Deleted ? "Deleted" : "Request Is NULL" });

            return Ok(new ResponseClass() { success = true, result = request_Type });
        }

        public async Task<IHttpActionResult> Edit(Request_Type request_Type)
        {
            var data = db.Request_Type.FirstOrDefault(q => q.Request_Type_ID == request_Type.Request_Type_ID && !q.Deleted);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            try
            {
                data.Request_Type_Name_AR = request_Type.Request_Type_Name_AR;
                data.Request_Type_Name_EN = request_Type.Request_Type_Name_EN;
                data.Desc_EN = request_Type.Desc_EN;
                data.Desc_AR = request_Type.Desc_AR;
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.RequestType, Method: "Create", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Request_Type_ID, notes: null);
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
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        public async Task<IHttpActionResult> Create(RequestTypeDTO request_Type)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();

            try
            {
                var path = HttpContext.Current.Server.MapPath("~");
                var FilePath = Path.Combine("Images", "Request_Type", DateTime.Now.Ticks.ToString() + ".png");

                var data = new Request_Type() { IS_Action = true, Deleted = false, Request_Type_Name_AR = request_Type.Request_Type_Name_AR, Request_Type_Name_EN = request_Type.Request_Type_Name_EN, Image_Path = FilePath.Replace('\\', '/'), Desc_AR = request_Type.Desc_AR, Desc_EN = request_Type.Desc_EN };

                db.Request_Type.Add(data);

                File.WriteAllBytes(Path.Combine(path, FilePath), Convert.FromBase64String(request_Type.Base64));
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.RequestType, Method: "Create", Oldval: null, Newval: data, es: out _, syslog: out _, ID: data.Request_Type_ID, notes: null);
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
                    throw new Exception();
                }
            }
            catch (Exception ee)
            {
                trans.Rollback();
                try
                {
                    var path = HttpContext.Current.Server.MapPath("~");
                    var FilePath = Path.Combine("Images", "Request_Type", DateTime.Now.Ticks.ToString() + ".png");
                    File.Delete(Path.Combine(path, FilePath));
                }
                catch (Exception)
                {
                }
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            var trans = db.Database.BeginTransaction();

            Request_Type request_Type = db.Request_Type.FirstOrDefault(q => q.Request_Type_ID == id);
            if (request_Type == null || request_Type.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });
            request_Type.IS_Action = false;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.RequestType, Method: "Deactive", Oldval: null, Newval: request_Type, es: out _, syslog: out _, ID: request_Type.Request_Type_ID, notes: null);
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
        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            var trans = db.Database.BeginTransaction();

            Request_Type request_Type = db.Request_Type.FirstOrDefault(q => q.Request_Type_ID == id);
            if (request_Type == null || request_Type.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });
            request_Type.IS_Action = true;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.RequestType, Method: "Active", Oldval: null, Newval: request_Type, es: out _, syslog: out _, ID: request_Type.Request_Type_ID, notes: null);
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
        [HttpPost]
        public async Task<IHttpActionResult> _Delete(int id)
        {

            Request_Type request_Type = db.Request_Type.Include(q => q.Units_Request_Type).Include(q => q.Request_Data).FirstOrDefault(q => q.Request_Type_ID == id && !q.Deleted);

            if (request_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });
            if (request_Type.Request_Data.Count == 0 && request_Type.Units_Request_Type.Count == 0)
            {
                var trans = db.Database.BeginTransaction();

                var OldVals = JsonConvert.SerializeObject(request_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                request_Type.Deleted = true;
                request_Type.DeletedAt = DateTime.Now;
                //p.Request_Type.Remove(request_Type);
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.RequestType, Method: "Delete", Oldval: OldVals, Newval: request_Type, es: out _, syslog: out _, ID: request_Type.Request_Type_ID, notes: null);
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
            return Ok(new ResponseClass() { success = false });
        }
        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            Request_Type request_Type = db.Request_Type.Include(q => q.Units_Request_Type.Select(s => s.Units)).FirstOrDefault(q => q.Request_Type_ID == id && q.Deleted);
            if (request_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Request Is NULL" });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(request_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            request_Type.Deleted = false;

            #region Delete UnitRequestType
            var UnitReqType = request_Type.Units_Request_Type;
            foreach (var UnitReq in UnitReqType)
            {
                if (!UnitReq.Units.Deleted)
                {
                    UnitReq.Deleted = false;
                }
            }
            //p.Units_Request_Type.RemoveRange(p.Units_Request_Type.Where(q => q.Units_ID == id).ToList());

            #endregion
            //p.Request_Type.Remove(request_Type);
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.RequestType, Method: "Restore", Oldval: OldVals, Newval: request_Type, es: out _, syslog: out _, ID: request_Type.Request_Type_ID, notes: null);
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}