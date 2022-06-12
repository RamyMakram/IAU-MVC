using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
    public class GeneralSettingController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();
        [HttpGet]
        public async Task<IHttpActionResult> Init()
        {
            var data = db.Request_State.Select(s => new { s.State_ID, s.StateName_AR, s.StateName_EN, s.AllowedDelay });
            return Ok(new ResponseClass() { success = true, result = new { Request_State = data, Use_SMS = WebApiApplication.Setting_UseMessage } });
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveDelayeValue([FromBody] List<Request_State> request_States)
        {
            var trans = db.Database.BeginTransaction();
            try
            {
                var states_ids = request_States.Select(s => (int)s.State_ID).ToArray();
                var OldVals = db.Request_State.Where(q => states_ids.Contains(q.State_ID)).ToList();
                foreach (var i in request_States)
                {
                    var data = db.Request_State.FirstOrDefault(q => q.State_ID == i.State_ID);
                    data.AllowedDelay = i.AllowedDelay;
                }
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db: db, logClass: LogClassType.General_Setting, Method: "Update", Oldval: OldVals, Newval: request_States, es: out _, syslog: out _, ID: null, notes: "Update Delayed Request Interval");
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
            catch (Exception eeee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }
        [HttpPost]
        public async Task<IHttpActionResult> UpdateSMS(bool value)
        {
            try
            {
                Configuration webConfigApp = WebConfigurationManager.OpenWebConfiguration("~");

                var OldVals = webConfigApp.AppSettings.Settings["Use_Message"].Value;

                webConfigApp.AppSettings.Settings["Use_Message"].Value = value.ToString();
                webConfigApp.Save();
                WebApiApplication.Setting_UseMessage = value;
                var logstate = Logger.AddLog(db: db, logClass: LogClassType.General_Setting, Method: "Update", Oldval: OldVals, Newval: value.ToString(), es: out _, syslog: out _, ID: null, notes: "Update SMS Enable or disable");
                if (logstate)
                {
                    await db.SaveChangesAsync();
                    return Ok(new ResponseClass() { success = true });
                }
                else
                {
                    return Ok(new ResponseClass() { success = false });
                }
            }
            catch (Exception)
            {
                return Ok(new ResponseClass { success = false });
            }
        }
    }
}
