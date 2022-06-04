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
using LinqKit;

namespace IAUBackEnd.Admin.Controllers
{
    public class Main_ServicesController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetDeleted()
        {
            return Ok(new ResponseClass() { success = true, result = db.Main_Services.Include(q => q.Service_Type).Where(q => q.Deleted) });
        }
        public async Task<IHttpActionResult> GetMain_Services()
        {
            return Ok(new ResponseClass() { success = true, result = db.Main_Services.Include(q => q.Service_Type).Where(q => !q.Deleted) });
        }
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Main_Services.Where(q => q.IS_Action.Value && !q.Deleted) });
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetActiveWithServiceTypeAndUnit(int id, [FromBody] List<int> servicetype)
        {
            var pred = PredicateBuilder.New<Main_Services>();
            var Unit = db.Units.FirstOrDefault(q => q.Units_ID == id && !q.Deleted);
            foreach (var i in servicetype)
                pred.Or(q => q.ServiceTypeID == i);
            pred.Or(q => q.ServiceTypeID == Unit.ServiceTypeID);
            var data = db.Main_Services.Where(q => q.IS_Action.Value && !q.Deleted).Where(pred).Select(q => new { q.Main_Services_ID, q.Main_Services_Name_AR, q.ServiceTypeID, q.Main_Services_Name_EN, Active = q.UnitMainServices.Count(w => w.UnitID == id) > 0 });
            return Ok(new ResponseClass() { success = true, result = data });
        }

        public async Task<IHttpActionResult> GetMain_Services(int id)
        {
            var main_Services = db.Main_Services.Include(q => q.ValidTo).Include(q => q.Service_Type).Where(q => q.Main_Services_ID == id && !q.Deleted).Select(q => new { q.Main_Services_ID, q.Main_Services_Name_AR, q.Main_Services_Name_EN, q.IS_Action, q.Service_Type, q.ServiceTypeID, q.ValidTo, MainService_ApplicantType = q.ValidTo.Select(w => new { w.Applicant_Type.Applicant_Type_Name_AR, w.Applicant_Type.Applicant_Type_Name_EN }) }).FirstOrDefault();
            if (main_Services == null)
                return Ok(new ResponseClass() { success = false, result = "Main Is Null" });

            return Ok(new ResponseClass() { success = true, result = main_Services });
        }

        public async Task<IHttpActionResult> Update(Main_Services main_Services)
        {
            var data = db.Main_Services.Include(q => q.ValidTo).FirstOrDefault(q => q.Main_Services_ID == main_Services.Main_Services_ID && !q.Deleted);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                data.Main_Services_Name_AR = main_Services.Main_Services_Name_AR;
                data.Main_Services_Name_EN = main_Services.Main_Services_Name_EN;
                data.ServiceTypeID = main_Services.ServiceTypeID;
                db.ValidTo.RemoveRange(data.ValidTo);
                data.ValidTo = main_Services.ValidTo;
                data.IS_Action = true;
                await db.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        public async Task<IHttpActionResult> Create(Main_Services main_Services)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                main_Services.IS_Action = true;
                main_Services.Deleted = false;
                db.Main_Services.Add(main_Services);
                await db.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            Main_Services main_Services = await db.Main_Services.FindAsync(id);
            if (main_Services == null || main_Services.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Main Is Null" });

            main_Services.IS_Action = false;
            await db.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
        }

        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            Main_Services main_Services = await db.Main_Services.FindAsync(id);
            if (main_Services == null || main_Services.Deleted)
                return Ok(new ResponseClass() { success = false, result = "Main Is Null" });

            main_Services.IS_Action = true;
            await db.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Delete(int id)
        {
            Main_Services main_Services = db.Main_Services.Include(q => q.Sub_Services.Select(s => s.Request_Data)).Include(q => q.UnitMainServices.Select(s => s.Units)).Include(q => q.UnitMainServices).FirstOrDefault(q => q.Main_Services_ID == id && !q.Deleted);
            if (main_Services == null)
                return Ok(new ResponseClass() { success = false, result = "Main Is Null" });
            if (main_Services.UnitMainServices.Count == 0 && main_Services.Sub_Services.All(q => q.Request_Data.Count == 0 && q.Deleted))
            {
                var subserviceid = main_Services.Sub_Services.Select(s => s.Sub_Services_ID);

                #region Delete ValidTo
                var VaildTo = db.ValidTo.Where(q => q.MainServiceID == id);
                foreach (var i in VaildTo)
                {
                    i.Deleted = true;
                    i.DeletedAt = DateTime.Now;
                }
                //db.ValidTo.RemoveRange(db.ValidTo.Where(q => q.MainServiceID == id));

                #endregion

                main_Services.Deleted = true;
                main_Services.DeletedAt = DateTime.Now;

                //db.Main_Services.Remove(main_Services);
                await db.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            return Ok(new ResponseClass()
            {
                success = false
            });
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            Main_Services main_Services = db.Main_Services.Include(q => q.Sub_Services.Select(s => s.Request_Data)).Include(q => q.UnitMainServices.Select(s => s.Units)).Include(q => q.UnitMainServices).FirstOrDefault(q => q.Main_Services_ID == id && q.Deleted);
            if (main_Services == null)
                return Ok(new ResponseClass() { success = false, result = "Main Is Null" });
            var subserviceid = main_Services.Sub_Services.Select(s => s.Sub_Services_ID);

            #region Delete ValidTo
            var VaildTo = db.ValidTo.Where(q => q.MainServiceID == id);
            foreach (var i in VaildTo)
            {
                i.Deleted = false;
            }
            //db.ValidTo.RemoveRange(db.ValidTo.Where(q => q.MainServiceID == id));

            #endregion

            main_Services.Deleted = false;

            //db.Main_Services.Remove(main_Services);
            await db.SaveChangesAsync();
            return Ok(new ResponseClass() { success = true });
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