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
    public class UnitTypesController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetUnits_Type()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units_Type });
        }
        public async Task<IHttpActionResult> GetUnits_TypeOFLevel(int id)
        {
            return Ok(new ResponseClass() { success = true, result = db.Units_Type.Where(q => q.LevelID == id) });
        }

        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Units_Type.Where(q => q.IS_Action.Value) });
        }
        public async Task<IHttpActionResult> GetUnits_TypeOFUnit(int id)
        {
            var units_Type = await db.Units.Include(q => q.Units_Type).FirstOrDefaultAsync(q => q.Units_ID == id && !q.Deleted);
            if (units_Type.Units_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });

            return Ok(new ResponseClass() { success = true, result = units_Type.Units_Type });
        }
        public async Task<IHttpActionResult> GetUnits_Type(int id)
        {
            Units_Type units_Type = await db.Units_Type.FindAsync(id);
            if (units_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });

            return Ok(new ResponseClass() { success = true, result = units_Type });
        }

        public async Task<IHttpActionResult> Edit(Units_Type units_Type)
        {
            var data = db.Units_Type.FirstOrDefault(q => q.Units_Type_ID == units_Type.Units_Type_ID);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(data, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            try
            {
                data.Units_Type_Name_AR = units_Type.Units_Type_Name_AR;
                data.Units_Type_Name_EN = units_Type.Units_Type_Name_EN;
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Update", Oldval: OldVals, Newval: data, es: out _, syslog: out _, ID: data.Units_Type_ID, notes: null);
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

        public async Task<IHttpActionResult> Create(Units_Type units_Type)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();

            units_Type.IS_Action = true;
            db.Units_Type.Add(units_Type);
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Create", Oldval: null, Newval: units_Type, es: out _, syslog: out _, ID: units_Type.Units_Type_ID, notes: null);
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
            Units_Type units_Type = await db.Units_Type.FindAsync(id);
            if (units_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(units_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            units_Type.IS_Action = false;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Deactive", Oldval: OldVals, Newval: units_Type, es: out _, syslog: out _, ID: units_Type.Units_Type_ID, notes: null);
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
            Units_Type units_Type = await db.Units_Type.FindAsync(id);
            if (units_Type == null)
                return Ok(new ResponseClass() { success = false, result = "Type Is NULL" });
            var trans = db.Database.BeginTransaction();
            var OldVals = JsonConvert.SerializeObject(units_Type, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            units_Type.IS_Action = true;
            await db.SaveChangesAsync();

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Unit, Method: "Active", Oldval: OldVals, Newval: units_Type, es: out _, syslog: out _, ID: units_Type.Units_Type_ID, notes: null);
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