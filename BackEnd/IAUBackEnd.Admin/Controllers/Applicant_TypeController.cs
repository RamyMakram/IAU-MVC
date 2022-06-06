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

namespace IAUBackEnd.Admin.Controllers
{
    public class Applicant_TypeController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();
        public async Task<IHttpActionResult> GetActive()
        {
            return Ok(new ResponseClass() { success = true, result = db.Applicant_Type.Where(q => q.IS_Action == true) });
        }
        public async Task<IHttpActionResult> GetDate()
        {
            return Ok(new ResponseClass() { success = true, result = Helper.GetDate() });
        }
        public async Task<IHttpActionResult> GetApplicant_Type()
        {
            return Ok(new ResponseClass() { success = true, result = db.Applicant_Type });
        }

        // GET: api/Applicant_Type1/5
        [ResponseType(typeof(Applicant_Type))]
        public async Task<IHttpActionResult> GetApplicant_Type(int id)
        {
            Applicant_Type applicant_Type = await db.Applicant_Type.FindAsync(id);
            if (applicant_Type == null)
                return Ok(new ResponseClass() { success = false });

            return Ok(new ResponseClass() { success = true, result = applicant_Type });
        }

        // PUT: api/Applicant_Type1/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateApplicant_Type(Applicant_Type applicant_Type)
        {
            var logstate = Logger.AddLog(db, LogClassType.ApplicantType, "Update", out _, out _, db.Applicant_Type.AsNoTracking().FirstOrDefault(q => q.Applicant_Type_ID == applicant_Type.Applicant_Type_ID), applicant_Type);
            if (!ModelState.IsValid || !logstate)
                return Ok(new ResponseClass() { success = false });
            try
            {
                db.Entry(applicant_Type).State = EntityState.Modified;
                db.Entry(applicant_Type).Property(q => q.IS_Action).IsModified = false;
                await db.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception cc)
            {
                return Ok(new ResponseClass() { success = false });
            }
        }

        [ResponseType(typeof(Applicant_Type))]
        public async Task<IHttpActionResult> Create(Applicant_Type applicant_Type)
        {
            var trans = db.Database.BeginTransaction();
            applicant_Type.Applicant_Type_ID = 0;
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false });

            try
            {
                applicant_Type.IS_Action = true;
                db.Applicant_Type.Add(applicant_Type);
                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db, LogClassType.ApplicantType, "Create", out _, out _, null, applicant_Type, applicant_Type.Applicant_Type_ID);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass() { success = true });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }

            }
            catch (Exception cc)
            {
                return Ok(new ResponseClass() { success = false });
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            try
            {
                var trans = db.Database.BeginTransaction();

                Applicant_Type applicant_Type = await db.Applicant_Type.FindAsync(id);
                if (applicant_Type == null)
                    return Ok(new ResponseClass() { success = false });
                applicant_Type.IS_Action = true;

                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db, LogClassType.ApplicantType, "Active", out _, out _, null, applicant_Type, applicant_Type.Applicant_Type_ID);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass() { success = true });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }

            }
            catch (Exception rr)
            {
                return Ok(new ResponseClass() { success = false });
            }
        }
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            try
            {
                var trans = db.Database.BeginTransaction();

                Applicant_Type applicant_Type = await db.Applicant_Type.FindAsync(id);
                if (applicant_Type == null)
                    return Ok(new ResponseClass() { success = false });
                applicant_Type.IS_Action = false;
                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db, LogClassType.ApplicantType, "Deactive", out _, out _, null, applicant_Type, applicant_Type.Applicant_Type_ID);
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    trans.Commit();
                    return Ok(new ResponseClass() { success = true });
                }
                else
                {
                    trans.Rollback();
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception rr)
            {
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