using IAUBackEnd.Admin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IAUBackEnd.Admin
{
    public enum LogClassType
    {
        ApplicantType,
        E_form,
        General_Setting,
        Job,
        Locations,
        MainService,
        Priviliges,
        RequestType,
        Request,
        ServiceType,
        SubService,
        UnitLevel,
        Unit,
        UnitLocation,
        UnitType,
        User,
        EformApproval,
        UnitSignature
    }

    public class Logger
    {
        public static int UserID
        {
            get
            {
                return int.Parse(HttpContext.Current.Request.Headers["user"]?.ToString() ?? "-1");
            }
        }
        public static bool AddLog(MostafidDBEntities db, LogClassType logClass, string Method, out Exception es, out SystemLog syslog, object Oldval = null, object Newval = null, int? ID = null, string notes = null)
        {
            try
            {
                var date = GetDate();
                if (UserID == -1)
                    throw new Exception("User Is Null");
                syslog = new SystemLog { CallPath = HttpContext.Current.Request.RawUrl, Method = Method, TransDate = date, ClassType = ((int)logClass), Oldval = (Oldval != null && Oldval.GetType() == typeof(string) ? Oldval.ToString() : JsonConvert.SerializeObject(Oldval, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore })), Newval = JsonConvert.SerializeObject(Newval, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), UserID = UserID, ReferID = ID, Notes = notes };
                db.SystemLog.Add(syslog);
                es = null;
                return true;
            }
            catch (Exception ee)
            {
                es = ee;
                syslog = null;
                return false;
            }
        }
        public static DateTime GetDate()
        {
            return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Arabic Standard Time"));
        }
    }

}