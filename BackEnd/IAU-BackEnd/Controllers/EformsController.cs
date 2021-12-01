using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAU_BackEnd.Controllers
{
    public class EformsController : ApiController
    {
        private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
        [HttpGet]
        [Route("api/Eforms/GetE_Form/{id}/{uid}")]
        public async Task<IHttpActionResult> GetE_Forms(int id, int uid)
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
                      Eform_Approval = q.Eform_Approval.Select(s => new { AR = s.Units.Units_Name_AR, EN = s.Units.Units_Name_EN }),
                      Question = q.Question.Where(s => s.Active).OrderBy(d => d.Index_Order).Select(s =>
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
                                new IAU.DTO.Entity.Input_Type
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
                var unit = await p.Units.FirstOrDefaultAsync(q => q.Units_ID == uid);
                return Ok(new ResponseClass() { success = true, result = new { Eform = e_Forms, UnitEN = unit.Units_Name_EN, UnitAR = unit.Units_Name_AR } });
            }
            catch (Exception eee)
            {
                return Ok(new ResponseClass() { success = false });
            }

        }
        [HttpGet]
        [Route("api/Eforms/GetE_FormsReadOnly/{id}")]
        public async Task<IHttpActionResult> GetE_FormsReadOnly(int id)
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
                      Eform_Approval = q.Eform_Approval.Select(s => new { AR = s.Units.Units_Name_AR, EN = s.Units.Units_Name_EN }),
                      Question = q.Question.Where(s => s.Active).OrderBy(d => d.Index_Order).Select(s =>
                          new
                          {
                              s.ID,
                              Name = s.LableName,
                              Name_EN = s.LableName_EN,
                              T = s.Type,
                              s.Requird,
                              Ref = s.RefTo,
                              s.Index_Order
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
        [HttpGet]
        [Route("api/Eforms/E_Forms/{SubService}")]
        public async Task<IHttpActionResult> E_Forms(int SubService)
        {
            return Ok(new ResponseClass() { success = true, result = p.E_Forms.Where(q => q.IS_Action && q.SubServiceID == SubService).Select(q => new { q.ID, q.Name, q.Name_EN }) });
        }
    }
}
