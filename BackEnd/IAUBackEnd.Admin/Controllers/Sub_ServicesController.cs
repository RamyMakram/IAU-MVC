using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class Sub_ServicesController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = db.Sub_Services.Where(q => q.Deleted) });
        }
        public async Task<IHttpActionResult> GetSub_Services()
        {
            return Ok(new ResponseClass() { success = true, result = db.Sub_Services.Where(q => !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetSub_ServicesByMain(int id)
        {
            return Ok(new ResponseClass() { success = true, result = db.Sub_Services.Where(q => q.Main_Services_ID == id && !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Sub_Services.Where(q => q.IS_Action.Value && !q.Deleted) });
        }

        public async Task<IHttpActionResult> GetSub_Services(int id)
        {
            Sub_Services sub_Services = db.Sub_Services.Include(q => q.Required_Documents).Include(q => q.Main_Services).Include(q => q.Main_Services.Service_Type).FirstOrDefault(q => q.Sub_Services_ID == id && !q.Deleted && !q.Main_Services.Deleted && !q.Main_Services.Service_Type.Deleted);
            if (sub_Services == null)
                return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });
            return Ok(new ResponseClass() { success = true, result = sub_Services });
        }

        public async Task<IHttpActionResult> Update(Sub_Services sub_Services)
        {
            var data = db.Sub_Services.Include(q => q.Required_Documents).FirstOrDefault(q => q.Sub_Services_ID == sub_Services.Sub_Services_ID && !q.Deleted);
            if (!ModelState.IsValid || data == null || db.Main_Services.Find(sub_Services.Main_Services_ID).Deleted)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(sub_Services, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            try
            {
                data.Sub_Services_Name_AR = sub_Services.Sub_Services_Name_AR;
                data.Sub_Services_Name_EN = sub_Services.Sub_Services_Name_EN;
                data.Main_Services_ID = sub_Services.Main_Services_ID;
                foreach (var i in sub_Services.Required_Documents)
                {
                    if (i.ID == null)
                        data.Required_Documents.Add(i);
                    else
                    {
                        var ss = data.Required_Documents.FirstOrDefault(q => q.ID == i.ID);
                        ss.Name_AR = i.Name_AR;
                        ss.Name_EN = i.Name_EN;
                        ss.IS_Action = i.IS_Action;
                        ss.Deleted = false;
                    }
                }
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.SubService, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Sub_Services_ID, notes: null);
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

        public async Task<IHttpActionResult> Create(Sub_Services sub_Services)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();

            try
            {
                sub_Services.IS_Action = true;
                sub_Services.Deleted = false;
                db.Sub_Services.Add(sub_Services);
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.SubService, Method: "Create", Oldval: null, Newval: sub_Services, es: out _, syslog: out _, ID: sub_Services.Sub_Services_ID, notes: null);
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

        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            Sub_Services sub_Services = await db.Sub_Services.FindAsync(id);
            if (sub_Services == null || sub_Services.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });
            var trans = db.Database.BeginTransaction();

            sub_Services.IS_Action = true;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.SubService, Method: "Active", Oldval: null, Newval: sub_Services, es: out _, syslog: out _, ID: sub_Services.Sub_Services_ID, notes: null);
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
        public async Task<IHttpActionResult> Deactive(int id)
        {
            Sub_Services sub_Services = await db.Sub_Services.FindAsync(id);
            if (sub_Services == null || sub_Services.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });
            var trans = db.Database.BeginTransaction();

            sub_Services.IS_Action = false;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.SubService, Method: "Deactive", Oldval: null, Newval: sub_Services, es: out _, syslog: out _, ID: sub_Services.Sub_Services_ID, notes: null);
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
            Sub_Services sub_Services = db.Sub_Services.Include(q => q.Request_Data).Include(q => q.E_Forms).FirstOrDefault(q => q.Sub_Services_ID == id && !q.Deleted);
            if (sub_Services == null)
                return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });
            if (sub_Services.E_Forms.Count(s => !s.Deleted) == 0 && sub_Services.Request_Data.Count == 0)
            {
                var trans = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(sub_Services, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


                #region Delete RequiredDocs 
                var RequiredDocs = db.Required_Documents.Where(q => q.SubServiceID == id);
                foreach (var i in RequiredDocs)
                {
                    i.Deleted = true;
                    i.DeletetAt = DateTime.Now;
                }
                //p.Required_Documents.RemoveRange(p.Required_Documents.Where(q => q.SubServiceID == id));
                #endregion

                sub_Services.Deleted = true;
                sub_Services.DeletedAt = DateTime.Now;
                //p.Sub_Services.Remove(sub_Services);
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.SubService, Method: "Delete", Oldval: null, Newval: sub_Services, es: out _, syslog: out _, ID: sub_Services.Sub_Services_ID, notes: null);
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
            return Ok(new ResponseClass() { success = false, result = "CantRemove" });
        }
        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            Sub_Services sub_Services = db.Sub_Services.Include(q => q.Request_Data).Include(q => q.E_Forms).FirstOrDefault(q => q.Sub_Services_ID == id && q.Deleted);
            if (sub_Services == null)
                return Ok(new ResponseClass() { success = false, result = "Service Is NULL" });
          
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(sub_Services, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


            #region Delete RequiredDocs 
            var RequiredDocs = db.Required_Documents.Where(q => q.SubServiceID == id);
            foreach (var i in RequiredDocs)
            {
                i.Deleted = false;
            }
            #endregion

            sub_Services.Deleted = false;
            //p.Sub_Services.Remove(sub_Services);
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.SubService, Method: "Restore", Oldval: null, Newval: sub_Services, es: out _, syslog: out _, ID: sub_Services.Sub_Services_ID, notes: null);
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