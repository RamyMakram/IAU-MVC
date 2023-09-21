using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.UI.WebControls;
using IAU.Shared;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;

namespace IAUBackEnd.Admin.Controllers
{
    public class GeneralSettingController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();
        [HttpGet]
        public async Task<IHttpActionResult> Init(bool isar)
        {
            var data = db.Request_State.Select(s => new { s.State_ID, s.StateName_AR, s.StateName_EN, s.AllowedDelay });
            var NewAndFlollowRequestLogin_Current = await db.AppSetting.FirstOrDefaultAsync(q => q.Key == AppSettingEnum.NewAndFlollowRequestLogin.ToString());

            var _NewAndFlollowRequestLogin = Enum.GetNames(typeof(NewAndFlollowRequestLoginEnum));
            Dictionary<string, string> NewAndFlollowRequestLogin = new Dictionary<string, string>();

            foreach (var i in _NewAndFlollowRequestLogin)
                NewAndFlollowRequestLogin.Add(i, ((NewAndFlollowRequestLoginEnum)Enum.Parse(typeof(NewAndFlollowRequestLoginEnum), i)).Translate(isar));

            return Ok(new ResponseClass() { success = true, result = new { Request_State = data, Use_SMS = WebApiApplication.Setting_UseMessage, NewAndFlollowRequestLogin, NewAndFlollowRequestLogin_Current = NewAndFlollowRequestLogin_Current?.Value } });
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
                var appsetting_value = await db.AppSetting.FirstOrDefaultAsync(q => q.Key == AppSettingEnum.UseMessages.ToString());

                string OldVals = "";

                if (appsetting_value == null)
                    db.AppSetting.Add(new AppSetting { Key = AppSettingEnum.UseMessages.ToString(), Value = value.ToString() });
                else
                {
                    OldVals = appsetting_value.Value.ToString();

                    appsetting_value.Value = value.ToString();
                }

                await db.SaveChangesAsync();


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

        [HttpPost]
        public async Task<IHttpActionResult> UpdateNewAndFlollowRequestLogin(string value)
        {
            try
            {

                var Enum_Values = Enum.GetNames(typeof(NewAndFlollowRequestLoginEnum));

                if (!Enum_Values.Contains(value))
                    return Ok(new ResponseClass() { success = false, result = "InvalidValue" });


                var appsetting_value = await db.AppSetting.FirstOrDefaultAsync(q => q.Key == AppSettingEnum.NewAndFlollowRequestLogin.ToString());

                string OldVals = "";

                if (appsetting_value == null)
                    db.AppSetting.Add(new AppSetting { Key = AppSettingEnum.NewAndFlollowRequestLogin.ToString(), Value = value.ToString() });
                else
                {
                    OldVals = appsetting_value.Value.ToString();

                    appsetting_value.Value = value.ToString();
                }

                await db.SaveChangesAsync();


                var logstate = Logger.AddLog(db: db, logClass: LogClassType.General_Setting, Method: "Update", Oldval: OldVals, Newval: value.ToString(), es: out _, syslog: out _, ID: null, notes: "Update NewAndFlollowRequestLogin");
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
