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

namespace IAUBackEnd.Admin.Controllers
{
    public class E_FormsController : ApiController
    {
        private MostafidDBEntities p = new MostafidDBEntities();

        public async Task<IHttpActionResult> GetE_Forms()
        {
            return Ok(new ResponseClass() { success = true, result = p.E_Forms.ToList() });
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

        public async Task<IHttpActionResult> Update(E_Forms e_Forms)
        {
            var data = p.E_Forms.FirstOrDefault(q => q.ID == e_Forms.ID);
            if (!ModelState.IsValid || data == null)
                return Ok(new ResponseClass() { success = false, result = ModelState });
            try
            {
                data.Name = e_Forms.Name;
                data.Name_EN = e_Forms.Name_EN;
                data.SubServiceID = e_Forms.SubServiceID;
                await p.SaveChangesAsync();
                return Ok(new ResponseClass() { success = true });
            }
            catch (DbUpdateConcurrencyException)
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
                var eform = new E_Forms() { SubServiceID = e_Forms.SubServiceID, Name_EN = e_Forms.Name_EN, Name = e_Forms.Name, IS_Action = true, CreatedOn = Helper.GetDate() };
                foreach (var i in e_Forms.Question)
                {
                    var quest = new Models.Question { Type = i.T, LableName = i.Name, LableName_EN = i.Name_EN, CreatedOn = Helper.GetDate(), Active = true, Requird = i.Requird };
                    switch (i.T)
                    {
                        case "I":
                        case "D":
                            quest.Input_Type = new Models.Input_Type { IsNumber = i.Input.ISNum, IsDate = i.Input.Date, Placeholder = i.Input.PlaceHolder, Placeholder_EN = i.Input.PlaceholderEN };
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