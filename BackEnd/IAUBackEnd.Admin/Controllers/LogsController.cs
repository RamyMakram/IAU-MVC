using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using IAUBackEnd.Admin.Models;
using System.Web.Http;
using IAUAdmin.DTO.Helper;
using System.Data.Entity.Core.Objects;

namespace IAUBackEnd.Admin.Controllers
{
    public class LogsController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();

        // GET: Logs
        public async Task<IHttpActionResult> GetAll()
        {
            var DateTime = Logger.GetDate().AddDays(-10);


            var data = db.SystemLog.Include(q => q.Users).Where(q => q.TransDate >= DateTime).Select(q => new { q.ClassType, q.TransDate, q.ID, q.UserID, q.Users, q.Notes, q.ReferID }).OrderByDescending(q => q.TransDate);

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Log, Method: "Get", Oldval: null, Newval: data, es: out _, syslog: out _, ID: null, notes: "Access Log System");
            if (logstate)
                await db.SaveChangesAsync();

            return Ok(new ResponseClass()
            {
                success = true,
                result = data
            });
        }

        //// GET: Logs/Details/5
        [HttpGet]
        public async Task<IHttpActionResult> Details(int? Did)
        {
            if (Did == null)
                return Ok(new ResponseClass()
                {
                    success = false
                });

            SystemLog systemLog = await db.SystemLog.Include(q => q.Users).FirstOrDefaultAsync(q => q.ID == Did);
            
            if (systemLog == null)
                return Ok(new ResponseClass()
                {
                    success = false
                });


            return Ok(new ResponseClass()
            {
                success = true,
                result = systemLog
            });
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
