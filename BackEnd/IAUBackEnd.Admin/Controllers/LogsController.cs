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
using IAUAdmin.DTO.Entity;
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


            var data = db.SystemLog.Include(q => q.Users).Where(q => q.TransDate >= DateTime).Select(q => new { q.ClassType, q.Method, q.TransDate, q.ID, q.UserID, q.Users, q.Notes, q.ReferID }).OrderByDescending(q => q.TransDate);

            var logstate = Logger.AddLog(db: db, logClass: LogClassType.Log, Method: "Get", Oldval: null, Newval: data, es: out _, syslog: out _, ID: null, notes: "Access Log System");
            if (logstate)
                await db.SaveChangesAsync();

            return Ok(new ResponseClass()
            {
                success = true,
                result = data
            });
        }

        //// GET: Logs/Detials/5
        [HttpGet]
        public async Task<IHttpActionResult> Details(int? Did, bool isar)
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

            var data = PrepairLog(systemLog, isar);

            return Ok(new ResponseClass()
            {
                success = true,
                result = data
            });
        }

        private LogDTO PrepairLog(SystemLog systemLog, bool isar)
        {
            var log = new LogDTO { ID = systemLog.ID, Method = systemLog.Method, TransDate = systemLog.TransDate, Notes = systemLog.Notes, ClassType = systemLog.ClassType, Users = new UserDTO { User_Name = systemLog.Users.User_Name }, UserID = systemLog.UserID, };
            string class_and_style = "class='Link-log' target='_blank'";
            if (systemLog.ReferID != null)
                switch (systemLog.ClassType)
                {
                    case ((int)LogClassType.ApplicantType):
                        {
                            var data = db.Applicant_Type.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/Applicanttype/Edit/{data.Applicant_Type_ID}'>{(isar ? data.Applicant_Type_Name_AR : data.Applicant_Type_Name_EN)}</a>";

                        }
                        break;
                    case ((int)LogClassType.E_form):
                        {
                            var data = db.E_Forms.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/Eforms/Eform?ID={data.ID}'>{(isar ? data.Name : data.Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.Job):
                        {
                            var data = db.Job.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/Jobs/Detials/{data.User_Permissions_Type_ID}'>{(isar ? (systemLog.Notes.Length > 3 ? "تعديل صلاحيات - " : "") + data.User_Permissions_Type_Name_AR : (systemLog.Notes.Length > 3 ? "Edit Privilges - " : "") + data.User_Permissions_Type_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.Locations):
                        {
                            log.Message = $@"<a {class_and_style} href='#'>{(isar ? "الاماكن" : "Locations")}</a>";
                        }
                        break;
                    case ((int)LogClassType.MainService):
                        {
                            var data = db.Main_Services.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/ServicesBank/Detials/{data.Main_Services_ID}'>{(isar ? data.Main_Services_Name_AR : data.Main_Services_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.Priviliges):
                        {
                            log.Message = $@"<a {class_and_style} href='#'>{(isar ? "الصلاحيات" : "Permisions")}</a>";
                        }
                        break;
                    case ((int)LogClassType.RequestType):
                        {
                            var data = db.Request_Type.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/RequestType/Detials/{data.Request_Type_ID}'>{(isar ? data.Request_Type_Name_AR : data.Request_Type_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.EformApproval):
                    case ((int)LogClassType.Request):
                        {
                            var mess = "";
                            switch (systemLog.Notes)
                            {
                                case "Read Request":
                                    mess = isar ? "قراءة الطلب" : "Read Request";
                                    break;
                                case "Coding Request":
                                    mess = isar ? "تكويد الطلب" : "Coding Request";
                                    break;
                                case "Forward Request":
                                    mess = isar ? "توجيه الطلب" : "Forward Request";
                                    break;
                                case "Reply to Request":
                                    mess = isar ? "الرد علي مستفيد" : "Reply to Mostafid";
                                    break;
                                case "Add Mostafid Reminder":
                                    mess = isar ? "اضافة تذكير" : "Add Reminder";
                                    break;
                                case "Archived Request":
                                    mess = isar ? " ارشفه" : "Archived Request";
                                    break;
                                case "Close Request":
                                    mess = isar ? "اغلاق وارشفه" : "Close Request";
                                    break;

                                default:
                                    break;
                            }
                            var data = db.Request_Data.Find(systemLog.ReferID);

                            log.Message = $@"<a {class_and_style} href='#'>{mess}</br>{data.Code_Generate}</br>{systemLog.ReferID}</a>";
                        }
                        break;
                    case ((int)LogClassType.ServiceType):
                        {
                            var data = db.Service_Type.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/ServiceTypes/Detials/{systemLog.ReferID}'>{(isar ? data.Service_Type_Name_AR : data.Service_Type_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.SubService):
                        {
                            var data = db.Sub_Services.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/SubServices/Detials/{systemLog.ReferID}'>{(isar ? data.Sub_Services_Name_AR : data.Sub_Services_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.UnitLevel):
                        {
                            var data = db.Sub_Services.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/Levels/Detials/{systemLog.ReferID}'>{(isar ? data.Sub_Services_Name_AR : data.Sub_Services_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.UnitSignature):
                    case ((int)LogClassType.Unit):
                        {
                            var data = db.Units.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/Units/Detials/{systemLog.ReferID}'>{(isar ? data.Units_Name_AR : data.Units_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.UnitLocation):
                        {
                            var data = db.Units_Location.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/UnitLocation/Detials/{systemLog.ReferID}'>{(isar ? data.Units_Location_Name_AR : data.Units_Location_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.UnitType):
                        {
                            var data = db.Units_Type.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/Units_Type/Detials/{systemLog.ReferID}'>{(isar ? data.Units_Type_Name_AR : data.Units_Type_Name_EN)}</a>";
                        }
                        break;
                    case ((int)LogClassType.User):
                        {
                            var data = db.Users.Find(systemLog.ReferID);
                            log.Message = $@"<a {class_and_style} href='/User/Detials/{systemLog.ReferID}'>{data.User_Email}</a>";
                        }
                        break;
                }
            if (systemLog.ClassType == (int)LogClassType.General_Setting)
                log.Message = $@"<a {class_and_style.Replace("target","")} href='#'>{(isar ? (log.Notes == "Update Delayed Request Interval" ? "تعديل الوقت المستغرق للطلبات المتاخرة" : "تفعيل او تعطيل رسائل ال SMS") : (log.Notes == "Update Delayed Request Interval" ? "Update Delayed Request Interval" : "Update SMS Enable or disable"))}</a>";

            return log;
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
