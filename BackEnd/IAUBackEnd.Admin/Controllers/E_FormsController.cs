using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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
using Newtonsoft.Json;

namespace IAUBackEnd.Admin.Controllers
{
    public class E_FormsController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetE_Forms()
        {
            return Ok(new ResponseClass() { success = true, result = p.E_Forms.ToList() });
        }
        public async Task<IHttpActionResult> GetE_FormsFoRequest(int id, int RequestID)
        {
            try
            {
                var data = p.Person_Eform.Include(q => q.E_Forms_Answer).FirstOrDefault(q => q.ID == id && q.Personel_Data.Request_Data.Any(s => s.Request_Data_ID == RequestID));
                if (data == null)
                    return Ok(new ResponseClass() { success = false });

                return Ok(new ResponseClass() { success = true, result = data });
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
                var e_Forms = p.E_Forms.Select(q =>
                  new
                  {
                      q.ID,
                      q.Name,
                      q.Name_EN,
                      q.Code,
                      q.SubServiceID,
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
            var e_Forms = p.E_Forms.Where(q => q.SubServiceID == id);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });

            return Ok(new ResponseClass() { success = true, result = e_Forms });
        }

        public async Task<IHttpActionResult> Update(E_FormsDTO e_Forms)
        {
            var eform = p.E_Forms.Include(q => q.Question).FirstOrDefault(q => q.ID == e_Forms.ID);
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
                    var item = p.Question.Find(i);
                    if (item != null)
                        p.Question.Remove(item);
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
                        quest = p.Question.Include(q => q.Paragraph).Include(q => q.Separator).Include(q => q.Input_Type).Include(q => q.Radio_Type).Include(q => q.CheckBox_Type).FirstOrDefault(q => q.ID == i.ID);
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
                                p.Radio_Type.RemoveRange(quest.Radio_Type.ToList());
                            foreach (var r in i.Radio)//If New Insert All
                            {
                                //if (newItem || r.ID == null)
                                quest.Radio_Type.Add(new Models.Radio_Type { Name = r.Name, Name_EN = r.Name_EN });
                                //else
                                //{
                                //    var rad = p.Radio_Type.FirstOrDefault(q => q.ID == r.ID);
                                //    if (rad == null)
                                //        quest.Radio_Type.Add(new Models.Radio_Type { Name = r.Name, Name_EN = r.Name_EN });
                                //    else
                                //    {
                                //        rad.Name = r.Name;
                                //        rad.Name_EN = r.Name_EN;
                                //    }
                                //}
                            }
                            break;
                        case "C":
                            if (!newItem)
                                p.CheckBox_Type.RemoveRange(quest.CheckBox_Type.ToList());
                            foreach (var c in i.Check)
                            {
                                //if (newItem || c.ID == null)
                                quest.CheckBox_Type.Add(new Models.CheckBox_Type { Name = c.Name, Name_EN = c.Name_EN });
                                //else
                                //{
                                //    var check = p.CheckBox_Type.FirstOrDefault(q => q.ID == c.ID);
                                //    if (check == null)
                                //        quest.CheckBox_Type.Add(new Models.CheckBox_Type { Name = c.Name, Name_EN = c.Name_EN });
                                //    else
                                //    {
                                //        check.Name = c.Name;
                                //        check.Name_EN = c.Name_EN;
                                //    }
                                //}
                            }
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

                    }
                    if (newItem)
                        eform.Question.Add(quest);
                }
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false });
            }
        }

        public async Task<IHttpActionResult> Create(E_FormsDTO e_Forms)
        {
            if (!ModelState.IsValid)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                var eform = new E_Forms() { SubServiceID = e_Forms.SubServiceID, Name_EN = e_Forms.Name_EN, Name = e_Forms.Name, IS_Action = true, CreatedOn = Helper.GetDate(), Code = e_Forms.Code };
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

                    }
                    eform.Question.Add(quest);
                }
                p.E_Forms.Add(eform);
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (Exception ee)
            {
                return Ok(new ResponseClass() { success = false, result = ee });
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> Active(int id)
        {
            E_Forms e_Forms = await p.E_Forms.FindAsync(id);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
            e_Forms.IS_Action = true;
            await p.SaveChangesAsync();

            return Ok(new ResponseClass() { success = true });
        }
        [HttpGet]
        public async Task<IHttpActionResult> Deactive(int id)
        {
            E_Forms e_Forms = await p.E_Forms.FindAsync(id);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
            e_Forms.IS_Action = false;
            await p.SaveChangesAsync();

            return Ok(new ResponseClass() { success = true });
        }

        [HttpPost]
        public async Task<IHttpActionResult> Delete(int id)
        {
            E_Forms e_Forms = p.E_Forms.FirstOrDefault(q => q.ID == id);
            if (e_Forms == null)
                return Ok(new ResponseClass() { success = false, result = "EForm IS NULL" });
            p.E_Forms.Remove(e_Forms);
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