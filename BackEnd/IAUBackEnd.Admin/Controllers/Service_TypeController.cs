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
    public class Service_TypeController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = db.Service_Type.Where(q => q.Deleted) });
        }
        public async Task<IHttpActionResult> GetService_Type()
        {
            return Ok(new ResponseClass() { success = true, result = db.Service_Type.Where(q => !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActiveService_TypeCharList()
        {
            try
            {
                var data = db.Service_Type.Where(q => q.IS_Action.Value && !q.Deleted).Select(q => q.Service_Type_Name_EN).ToList().Select(q => q.ElementAt(0));
                return Ok(new ResponseClass() { success = true, result = data });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Service_Type.Where(q => q.IS_Action.Value && !q.Deleted) });
        }

        public async Task<IHttpActionResult> GetService_Type(int id)
        {
            Service_Type service_Type = await db.Service_Type.FindAsync(id);
            if (service_Type == null || service_Type.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

            return Ok(new ResponseClass() { success = true, result = service_Type });
        }

        public async Task<IHttpActionResult> Update(Service_Type service_Type)
        {
            var data = db.Service_Type.FirstOrDefault(q => q.Service_Type_ID == service_Type.Service_Type_ID && !q.Deleted);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                var trans = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


                data.Service_Type_Name_AR = service_Type.Service_Type_Name_AR;
                data.Service_Type_Name_EN = service_Type.Service_Type_Name_EN;
                await db.SaveChangesAsync();


                var logstate = Logger.AddLog(db: db, logClass: LogClassType.ServiceType, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Service_Type_ID, notes: null);
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
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        public async Task<IHttpActionResult> Create(ServiceTypeDTO service_Type)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();

            var FilePath = Path.Combine("Images", "Service_Types", DateTime.Now.Ticks.ToString() + ".png");
            var path = HttpContext.Current.Server.MapPath("~");

            try
            {
                var new_ServiceType = new Service_Type() { IS_Action = true, Deleted = false, Service_Type_Name_AR = service_Type.Service_Type_Name_AR, Service_Type_Name_EN = service_Type.Service_Type_Name_EN, Image_Path = FilePath.Replace('\\', '/') };
                db.Service_Type.Add(new_ServiceType);
                File.WriteAllBytes(Path.Combine(path, FilePath), Convert.FromBase64String(service_Type.Base64));
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.ServiceType, Method: "Create", Oldval: null, Newval: new_ServiceType, es: out _, syslog: out _, ID: new_ServiceType.Service_Type_ID, notes: null);
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
                    try
                    {
                        if (File.Exists(Path.Combine(path, FilePath)))
                            File.Delete(Path.Combine(path, FilePath));
                    }
                    catch (Exception)
                    {
                    }
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception ee)
            {
                try
                {
                    if (File.Exists(Path.Combine(path, FilePath)))
                        File.Delete(Path.Combine(path, FilePath));
                }
                catch (Exception)
                {
                }
                trans.Rollback();
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            Service_Type service_Type = await db.Service_Type.FindAsync(id);
            if (service_Type == null || service_Type.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(service_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            service_Type.IS_Action = false;
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.ServiceType, Method: "Deactive", Oldval: OldVals, Newval: service_Type, es: out _, syslog: out _, ID: service_Type.Service_Type_ID, notes: null);
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
            Service_Type service_Type = await db.Service_Type.FindAsync(id);
            if (service_Type == null || service_Type.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(service_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            service_Type.IS_Action = true;
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.ServiceType, Method: "Active", Oldval: OldVals, Newval: service_Type, es: out _, syslog: out _, ID: service_Type.Service_Type_ID, notes: null);
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
            Service_Type service_Type = db.Service_Type.Include(q => q.Request_Data).Include(q => q.UnitServiceTypes).Include(q => q.Units).FirstOrDefault(q => q.Service_Type_ID == id && !q.Deleted);
            if (service_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });
            if (service_Type.Request_Data.Count == 0 && service_Type.UnitServiceTypes.Count == 0 && service_Type.Units.Count == 0 && service_Type.Main_Services.All(q => q.Deleted))
            {
                var trans = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(service_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


                service_Type.Deleted = true;
                service_Type.DeletedAt = DateTime.Now;

                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.ServiceType, Method: "Delete", Oldval: OldVals, Newval: service_Type, es: out _, syslog: out _, ID: service_Type.Service_Type_ID, notes: null);
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
            Service_Type service_Type = db.Service_Type.Include(q => q.UnitServiceTypes.Select(s => s.Units)).FirstOrDefault(q => q.Service_Type_ID == id && q.Deleted);
            if (service_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Type IS NULL" });

            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(service_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            service_Type.Deleted = false;

            #region Delete UnitServiceType
            var UnitServiceType = service_Type.UnitServiceTypes;
            foreach (var i in UnitServiceType)
            {
                if (!i.Units.Deleted)
                {
                    i.Deleted = false;
                }
            }

            #endregion
            //p.Service_Type.Remove(service_Type);
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.ServiceType, Method: "Restore", Oldval: OldVals, Newval: service_Type, es: out _, syslog: out _, ID: service_Type.Service_Type_ID, notes: null);
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