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
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class UnitsLocationController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units_Location.Where(q => q.Deleted) });
        }
        public async Task<IHttpActionResult> GetUnits_Location()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units_Location.Where(q => !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units_Location.Where(q => q.IS_Action.Value && !q.Deleted) });
        }

        public async Task<IHttpActionResult> GetUnits_Location(int id)
        {
            Units_Location units_Location = await db.Units_Location.FindAsync(id);
            if (units_Location == null || units_Location.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });

            return Ok(new ResponseClass() { success = true, result = units_Location });
        }
        public async Task<IHttpActionResult> GetLocationWithLang(int id, string lang)
        {
            var location = db.Units_Location.Include(q => q.Location).FirstOrDefault(q => q.Units_Location_ID == id && !q.Deleted);
            if (location == null)
                return Ok(new ResponseClass() { success = false, result = "Location Is Null" });

            return Ok(new ResponseClass() { success = true, result = location });
        }
        public async Task<IHttpActionResult> EditUnits_Location(Units_Location units_Location)
        {
            var data = db.Units_Location.FirstOrDefault(q => q.Units_Location_ID == units_Location.Units_Location_ID);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });

            var trans = db.Database.BeginTransaction();

            var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            try
            {
                data.Units_Location_Name_AR = units_Location.Units_Location_Name_AR;
                data.Units_Location_Name_EN = units_Location.Units_Location_Name_EN;
                data.Location_ID = units_Location.Location_ID;

                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitLocation, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Units_Location_ID, notes: null);
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

        public async Task<IHttpActionResult> Create(Units_Location units_Location)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });

            var trans = db.Database.BeginTransaction();

            units_Location.Deleted = false;
            units_Location.IS_Action = true;
            db.Units_Location.Add(units_Location);
            await db.SaveChangesAsync();


            var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitLocation, Method: "Create", Oldval: null, Newval: units_Location, es: out _, syslog: out _, ID: units_Location.Units_Location_ID, notes: null);
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
            Units_Location units_Location = await db.Units_Location.FindAsync(id);
            if (units_Location == null || units_Location.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });

            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(units_Location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


            units_Location.IS_Action = false;
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitLocation, Method: "Deactive", Oldval: OldVals, Newval: units_Location, es: out _, syslog: out _, ID: units_Location.Units_Location_ID, notes: null);
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
            Units_Location units_Location = await db.Units_Location.FindAsync(id);
            if (units_Location == null || units_Location.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });

            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(units_Location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });


            units_Location.IS_Action = true;
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitLocation, Method: "Active", Oldval: OldVals, Newval: units_Location, es: out _, syslog: out _, ID: units_Location.Units_Location_ID, notes: null);
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
            Units_Location units_Location = db.Units_Location.Include(q => q.Units).FirstOrDefault(q => q.Units_Location_ID == id && !q.Deleted);
            if (units_Location == null)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
            if (units_Location.Units.Count == 0)
            {
                var trans = db.Database.BeginTransaction();
                var OldVals = JsonConvert.SerializeObject(units_Location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                //p.Units_Location.Remove(units_Location);
                units_Location.Deleted = true;
                units_Location.DeletedAt = DateTime.Now;
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitLocation, Method: "Delete", Oldval: OldVals, Newval: units_Location, es: out _, syslog: out _, ID: units_Location.Units_Location_ID, notes: null);
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
            Units_Location units_Location = db.Units_Location.Include(q => q.Units).FirstOrDefault(q => q.Units_Location_ID == id && q.Deleted);
            if (units_Location == null)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(units_Location, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            //p.Units_Location.Remove(units_Location);
            units_Location.Deleted = false;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.UnitLocation, Method: "Restore", Oldval: OldVals, Newval: units_Location, es: out _, syslog: out _, ID: units_Location.Units_Location_ID, notes: null);
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