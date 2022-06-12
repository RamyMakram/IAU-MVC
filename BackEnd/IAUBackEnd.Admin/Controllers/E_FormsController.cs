using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using IAUAdmin.DTO.Entity;
using IAUAdmin.DTO.Helper;
using IAUBackEnd.Admin.Models;
using log4net;
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class E_FormsController : ApiController
    {
        private MostafidDBEntities db = new MostafidDBEntities();
        public async Task<IHttpActionResult> GetDeleted()
        {
            try
            {
                return Ok(new ResponseClass() { success = true, result = db.E_Forms.Where(q => q.Deleted) });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass
                {
                    success = false,
                    result = ee
                });
            }
        }
        public async Task<IHttpActionResult> GetE_Forms()
        {
            return Ok(new ResponseClass() { success = true, result = db.E_Forms.Where(q => !q.Deleted).ToList() });
        }
        public async Task<IHttpActionResult> GetE_FormsFoRequest(int id, int RequestID)
        {
            try
            {
                var data = await db.Person_Eform.Include(q => q.E_Forms_Answer)
                    .Where(q => q.ID == id && q.Personel_Data.Request_Data.Any(s => s.Request_Data_ID == RequestID))
                    .Select(q => new
                    {
                        q.ID,
                        q.Name,
                        q.Name_EN,
                        q.Person_ID,
                        q.FillDate,
                        q.Code,
                        E_Forms_Answer = q.E_Forms_Answer.Select(s => new { s.ID, s.Question_ID, s.EForm_ID, s.FillDate, s.Name, s.Name_En, T = s.Type, s.Value, s.Value_En, Preview_TableCols = s.Preview_TableCols.Select(d => new { d.Name, d.Name_En, d.Tables_Answare }), s.Index_Order }).OrderBy(r => r.Index_Order),
                        Eform_Approval = q.Preview_EformApproval.Select(s => new { AR = s.Name, EN = s.Name_En, s.UnitID, s.OwnEform, s.SignDate }),
                    }).FirstOrDefaultAsync();

                if (data == null)
                    return Ok(new ResponseClass() { success = false });

                var OwnUnit = data.Eform_Approval.FirstOrDefault(s => s.OwnEform);

                int uid = OwnUnit == null ? db.Request_Data.FirstOrDefault(q => q.Request_Data_ID == RequestID).Unit_ID.Value : OwnUnit.UnitID;
                var unit = await db.Units.Include(q => q.Unit_Signature).FirstOrDefaultAsync(q => q.Units_ID == uid);
                return Ok(new ResponseClass() { success = true, result = new { Eform = data, UnitEN = unit.Units_Name_EN, UnitAR = unit.Units_Name_AR, UnitCode = unit.Ref_Number.Substring(4) + " " + data.Code, Signature = unit.Unit_Signature } });
            }
            catch (Exception eee)
            {
                return Ok(new ResponseClass() { success = false });
            }
        }
        public async Task<IHttpActionResult> GetE_Forms(int id)
        {
            try
            {
                var e_Forms = db.E_Forms.Where(q => !q.Deleted).Select(q =>
                    new
                    {
                        q.ID,
                        q.Name,
                        q.Name_EN,
                        q.Code,
                        q.SubServiceID,
                        q.UnitToApprove,
                        Eform_Approval = q.Units,
                        Question = q.Question.OrderBy(d => d.Index_Order).Select(s =>
                          new
                          {
                              s.ID,
                              Name = s.LableName,
                              Name_EN = s.LableName_EN,
                              T = s.Type,
                              s.Requird,
                              Ref = s.RefTo,
                              s.Index_Order,
                              s.Active,
                              Sepa = s.Separator,
                              Para = s.Paragraph,
                              NRows = s.TableRowsNum,
                              Columns = s.Table_Columns.Select(d =>
                                  new
                                  {
                                      d.Name,
                                      d.Name_En,
                                      d.ID
                                  }),
                              Radio = s.Radio_Type.Select(e =>
                                 new
                                 {
                                     e.ID,
                                     e.Name_EN,
                                     e.Name
                                 }),
                              Check = s.CheckBox_Type.Select(e =>
                                  new
                                  {
                                      e.ID,
                                      e.Name_EN,
                                      e.Name
                                  }),
                              Input = s.Input_Type == null ? null :
                                  new IAUAdmin.DTO.Entity.Input_Type
                                  {
                                      ID = s.Input_Type.ID,
                                      Date = s.Input_Type.IsDate,
                                      ISNum = s.Input_Type.IsNumber,
                                      PlaceHolder = s.Input_Type.Placeholder,
                                      PlaceholderEN = s.Input_Type.Placeholder_EN
                                  }
                          })
                    }).FirstOrDefault(q => q.ID == id);
                if (e_Forms == null)
                    return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
                return Ok(new ResponseClass() { success = true, result = e_Forms });
            }
            catch (Exception eee)
            {
                return Ok(new ResponseClass() { success = false });
            }

        }
        public async Task<IHttpActionResult> GetE_FormsWithSubService(int id)
        {
            var e_Forms = db.E_Forms.Where(q => q.SubServiceID == id && !q.Deleted);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });

            return Ok(new ResponseClass() { success = true, result = e_Forms });
        }

        //public async Task<IHttpActionResult> UpdateTrack(E_Forms e_Forms)
        //{
        //    try
        //    {


        //        db.Entry(e_Forms).State = EntityState.Modified;
        //        await db.SaveChangesAsync();

        //    }
        //    catch (Exception ee)
        //    {
        //        return Ok(new ResponseClass() { success = false, result = ee });
        //    }
        //}
        public async Task<IHttpActionResult> Update(E_FormsDTO e_Forms)
        {
            var trans = db.Database.BeginTransaction();

            var eform = db.E_Forms.Include(q => q.Question).Include(q => q.Units).FirstOrDefault(q => q.ID == e_Forms.ID && !q.Deleted);
            var eform_old = JsonConvert.SerializeObject(eform, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            if (!ModelState.IsValid || eform == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                eform.Name = e_Forms.Name;
                eform.Name_EN = e_Forms.Name_EN;
                eform.SubServiceID = e_Forms.SubServiceID;
                eform.Code = e_Forms.Code;
                var Deleted_IDs = JsonConvert.DeserializeObject<List<int>>(e_Forms.Del_QTY);
                foreach (var i in Deleted_IDs)
                {
                    var item = db.Question.Find(i);
                    if (item != null)
                        db.Question.Remove(item);
                }
                e_Forms.Question = JsonConvert.DeserializeObject<List<IAUAdmin.DTO.Entity.Question>>(e_Forms.QTY);
                foreach (var i in e_Forms.Question)
                {
                    var newItem = true;
                    var quest = new Models.Question();
                    if (i.ID == null)
                        quest = new Models.Question { Type = i.T, LableName = i.Name ?? "", LableName_EN = i.Name_EN ?? "", CreatedOn = Helper.GetDate(), Active = true, Requird = i.Requird, Index_Order = i.Index_Order };
                    else
                    {
                        quest = db.Question.Include(q => q.Paragraph).Include(q => q.Separator).Include(q => q.Input_Type).Include(q => q.Radio_Type).Include(q => q.CheckBox_Type).Include(q => q.Table_Columns).FirstOrDefault(q => q.ID == i.ID);
                        if (quest == null)
                            quest = new Models.Question { Type = i.T, LableName = i.Name ?? "", LableName_EN = i.Name_EN ?? "", CreatedOn = Helper.GetDate(), Active = true, Requird = i.Requird, Index_Order = i.Index_Order };
                        else
                        {
                            newItem = false;
                            quest.LableName = i.Name ?? "";
                            quest.LableName_EN = i.Name_EN ?? "";
                            quest.Requird = i.Requird;
                            quest.Index_Order = i.Index_Order;
                        }
                    }
                    switch (i.T)
                    {
                        case "I":
                            if (newItem)
                                quest.Input_Type = new Models.Input_Type { IsNumber = i.Input.ISNum, IsDate = i.Input.Date, Placeholder = i.Input.PlaceHolder, Placeholder_EN = i.Input.PlaceholderEN };
                            else
                            {
                                quest.Input_Type.IsNumber = i.Input.ISNum;
                                quest.Input_Type.IsDate = i.Input.Date;
                                quest.Input_Type.Placeholder = i.Input.PlaceHolder;
                                quest.Input_Type.Placeholder_EN = i.Input.PlaceholderEN;
                            }
                            break;
                        case "D":
                            break;
                        case "R":
                            if (!newItem)
                                db.Radio_Type.RemoveRange(quest.Radio_Type.ToList());
                            foreach (var r in i.Radio)//If New Insert All
                                quest.Radio_Type.Add(new Models.Radio_Type { Name = r.Name, Name_EN = r.Name_EN });
                            break;
                        case "C":
                            if (!newItem)
                                db.CheckBox_Type.RemoveRange(quest.CheckBox_Type.ToList());
                            foreach (var c in i.Check)
                                quest.CheckBox_Type.Add(new Models.CheckBox_Type { Name = c.Name, Name_EN = c.Name_EN });
                            break;
                        case "P":
                            if (newItem)
                                quest.Paragraph = new Paragraph { Name = i.Para.Name, Name_En = i.Para.Name_En };
                            else
                            {
                                quest.Paragraph.Name = i.Para.Name;
                                quest.Paragraph.Name_En = i.Para.Name_En;
                            }
                            break;
                        case "S":
                            if (newItem)
                                quest.Separator = new Separator { IsEmpty = i.Sepa.Empty };
                            else
                                quest.Separator.IsEmpty = i.Sepa.Empty;
                            break;
                        case "T":
                            break;
                        case "E":
                            quest.RefTo = i.Ref;
                            break;
                        case "G":
                            if (!newItem)
                                db.Table_Columns.RemoveRange(quest.Table_Columns.ToList());
                            quest.TableRowsNum = i.NRows;
                            foreach (var r in i.Columns)//If New Insert All
                                quest.Table_Columns.Add(new Models.Table_Columns { Name = r.Name, Name_En = r.Name_En });
                            break;

                    }
                    if (newItem)
                        eform.Question.Add(quest);
                }
                eform.UnitToApprove = e_Forms.UnitToApprove;
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db, LogClassType.E_form, "Update", out _, out _, eform_old, eform, e_Forms.ID);
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
            catch (Exception ee)
            {
                try
                {

                    string validationErrors = "";
                    foreach (var failure in (ee as DbEntityValidationException).EntityValidationErrors)
                    {
                        foreach (var error in failure.ValidationErrors)
                            validationErrors += error.PropertyName + "  " + error.ErrorMessage;
                        validationErrors += "\n";
                    }
                    WebApiApplication.log.Error("Error In Update Eform with data\n" + JsonConvert.SerializeObject(e_Forms, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), ee);
                    WebApiApplication.log.Error(validationErrors);
                }
                catch (Exception)
                {
                    WebApiApplication.log.Error("Error In Update Eform with data\n" + JsonConvert.SerializeObject(e_Forms, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), ee);

                }
                trans.Rollback();

                return Ok(new ResponseClass() { success = false });
            }
        }

        public async Task<IHttpActionResult> Create(E_FormsDTO e_Forms)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            var trans = db.Database.BeginTransaction();

            var eform = new E_Forms() { SubServiceID = e_Forms.SubServiceID, Name_EN = e_Forms.Name_EN, Name = e_Forms.Name, IS_Action = true, CreatedOn = Helper.GetDate(), Code = e_Forms.Code };
            try
            {
                foreach (var i in e_Forms.Question)
                {
                    var quest = new Models.Question { Type = i.T, LableName = i.Name ?? "", LableName_EN = i.Name_EN ?? "", CreatedOn = Helper.GetDate(), Active = true, Requird = i.Requird, Index_Order = i.Index_Order };
                    switch (i.T)
                    {
                        case "I":
                            quest.Input_Type = new Models.Input_Type { IsNumber = i.Input.ISNum, IsDate = i.Input.Date, Placeholder = i.Input.PlaceHolder, Placeholder_EN = i.Input.PlaceholderEN };
                            break;
                        case "D":
                            quest.Input_Type = new Models.Input_Type { IsDate = true, Placeholder_EN = "", Placeholder = "" };
                            break;
                        case "R":
                            foreach (var r in i.Radio)
                                quest.Radio_Type.Add(new Models.Radio_Type { Name = r.Name, Name_EN = r.Name_EN });
                            break;
                        case "C":
                            foreach (var c in i.Check)
                                quest.CheckBox_Type.Add(new Models.CheckBox_Type { Name = c.Name, Name_EN = c.Name_EN });
                            break;
                        case "P":
                            quest.Paragraph = new Paragraph { Name = i.Para.Name, Name_En = i.Para.Name_En };
                            break;
                        case "S":
                            quest.Separator = new Separator { IsEmpty = i.Sepa.Empty };
                            break;
                        case "T":
                            break;
                        case "E":
                            quest.RefTo = i.Ref;
                            break;
                        case "G":
                            quest.TableRowsNum = i.NRows;
                            foreach (var col in i.Columns)
                                quest.Table_Columns.Add(new Models.Table_Columns { Name = col.Name, Name_En = col.Name_En });
                            break;
                    }
                    eform.Question.Add(quest);
                }
                eform.UnitToApprove = e_Forms.UnitToApprove;
                eform.Deleted = false;
                db.E_Forms.Add(eform);
                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db, LogClassType.E_form, "Create", out _, out _, null, eform, eform.ID);
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
            catch (Exception ee)
            {
                try
                {

                    string validationErrors = "";
                    foreach (var failure in (ee as DbEntityValidationException).EntityValidationErrors)
                    {
                        foreach (var error in failure.ValidationErrors)
                            validationErrors += error.PropertyName + "  " + error.ErrorMessage;
                        validationErrors += "\n";
                    }
                    WebApiApplication.log.Error("Error In Update Eform with data\n" + JsonConvert.SerializeObject(e_Forms, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), ee);
                    WebApiApplication.log.Error(validationErrors);
                }
                catch (Exception)
                {
                    WebApiApplication.log.Error("Error In Update Eform with data\n" + JsonConvert.SerializeObject(e_Forms, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }), ee);

                }
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });

            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            var trans = db.Database.BeginTransaction();

            E_Forms e_Forms = await db.E_Forms.FindAsync(id);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = e_Forms.Deleted ? "Deleted" : "EForm IS NULL" });
            e_Forms.IS_Action = true;
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db, LogClassType.E_form, "Active", out _, out _, null, null, e_Forms.ID);
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
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            var trans = db.Database.BeginTransaction();

            E_Forms e_Forms = await db.E_Forms.FindAsync(id);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = e_Forms.Deleted ? "Deleted" : "EForm IS NULL" });
            e_Forms.IS_Action = false;
            await db.SaveChangesAsync();
            var logstate = Logger.AddLog(db, LogClassType.E_form, "Deactive", out _, out _, null, null, e_Forms.ID);
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

        [HttpPost]
        public async Task<IHttpActionResult> Delete(int id)
        {
            var trans = db.Database.BeginTransaction();
            try
            {

                E_Forms e_Forms = db.E_Forms.FirstOrDefault(q => q.ID == id && !q.Deleted);
                if (e_Forms == null)
                    return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
                e_Forms.Deleted = true;
                e_Forms.DeletedAt = DateTime.Now;
                await db.SaveChangesAsync();

                var logstate = Logger.AddLog(db, LogClassType.E_form, "Delete", out _, out _, null, null, e_Forms.ID);
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
            catch (Exception ee)
            {
                trans.Rollback();
                return Ok(new ResponseClass() { success = false });
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> _Restore(int id)
        {
            var trans = db.Database.BeginTransaction();

            try
            {
                E_Forms e_Forms = db.E_Forms.FirstOrDefault(q => q.ID == id && q.Deleted);
                if (e_Forms == null)
                    return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
                e_Forms.Deleted = false;
                await db.SaveChangesAsync();
                var logstate = Logger.AddLog(db, LogClassType.E_form, "Restore", out _, out _, null, null, e_Forms.ID);
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
            catch (Exception ee)
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