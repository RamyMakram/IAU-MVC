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

namespace IAUBackEnd.Admin.Controllers
{
    public class UnitsLocationController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units_Location.Where(q => q.Deleted) });
        }
        public async Task<IHttpActionResult> GetUnits_Location()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units_Location.Where(q => !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = p.Units_Location.Where(q => q.IS_Action.Value && !q.Deleted) });
        }

        public async Task<IHttpActionResult> GetUnits_Location(int id)
        {
            Units_Location units_Location = await p.Units_Location.FindAsync(id);
            if (units_Location == null || units_Location.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });

            return Ok(new ResponseClass() { success = true, result = units_Location });
        }
        public async Task<IHttpActionResult> GetLocationWithLang(int id, string lang)
        {
            var location = p.Units_Location.Include(q => q.Location).FirstOrDefault(q => q.Units_Location_ID == id && !q.Deleted);
            if (location == null)
                return Ok(new ResponseClass() { success = false, result = "Location Is Null" });

            return Ok(new ResponseClass() { success = true, result = location });
        }
        public async Task<IHttpActionResult> EditUnits_Location(Units_Location units_Location)
        {
            var data = p.Units_Location.FirstOrDefault(q => q.Units_Location_ID == units_Location.Units_Location_ID);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                data.Units_Location_Name_AR = units_Location.Units_Location_Name_AR;
                data.Units_Location_Name_EN = units_Location.Units_Location_Name_EN;
                data.Location_ID = units_Location.Location_ID;
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        public async Task<IHttpActionResult> Create(Units_Location units_Location)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            units_Location.Deleted = false;
            units_Location.IS_Action = true;
            p.Units_Location.Add(units_Location);
            await p.SaveChangesAsync();

            return Ok(new ResponseClass() { success = true });
        }
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            Units_Location units_Location = await p.Units_Location.FindAsync(id);
            if (units_Location == null || units_Location.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
            units_Location.IS_Action = false;
            await p.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
        }
        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            Units_Location units_Location = await p.Units_Location.FindAsync(id);
            if (units_Location == null || units_Location.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
            units_Location.IS_Action = true;
            await p.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
        }
        [HttpPost]
        public async Task<IHttpActionResult> _Delete(int id)
        {
            Units_Location units_Location = p.Units_Location.Include(q => q.Units).FirstOrDefault(q => q.Units_Location_ID == id && !q.Deleted);
            if (units_Location == null)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
            if (units_Location.Units.Count == 0)
            {
                //p.Units_Location.Remove(units_Location);
                units_Location.Deleted = true;
                units_Location.DeletedAt = DateTime.Now;
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            return Ok(new ResponseClass() { success = false, result = "CantRemove" });
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            Units_Location units_Location = p.Units_Location.Include(q => q.Units).FirstOrDefault(q => q.Units_Location_ID == id && q.Deleted);
            if (units_Location == null)
                return Ok(new ResponseClass() { success = false, result = "Units Location Is Null" });
            //p.Units_Location.Remove(units_Location);
            units_Location.Deleted = false;
            await p.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                p.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}