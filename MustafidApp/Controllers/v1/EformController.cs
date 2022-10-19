using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MustafidAppDTO.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AutoMapper;
using MustafidAppModels.Context;
using Microsoft.EntityFrameworkCore;
using MustafidApp.Helpers;
using System.Linq;
using MustafidAppModels.Models;

namespace MustafidApp.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class EformController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        public EformController(MustafidAppContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSServices(IList<EformAnsDTO> ef_data, int EF_ID, bool IsUpdate, int? OldEFID)
        {
            var eform = await _appContext.EForms.FirstOrDefaultAsync(q => q.Id == EF_ID && !q.Deleted && q.IsAction.Value);
            if (eform == null)
                return Ok(new ResponseClass() { Success = false, data = "" });
            if (IsUpdate && OldEFID.HasValue)
            {
                _appContext.PersonEforms.Remove(_appContext.PersonEforms.FirstOrDefault(q => q.Id == OldEFID));
                await _appContext.SaveChangesAsync();
            }

            var EF_Model = _mapper.Map<IList<EFormsAnswer>>(eform);

            var Eform_Person = new PersonEform { Code = eform.Code, Name = eform.Name, NameEn = eform.NameEn, FillDate = DateTime.Now };

            foreach (var i in eform.Questions)
            {
                var Inser_Qty = EF_Model.FirstOrDefault(q => q.QuestionId == i.Id);
                if (i.Type == "S")
                {
                    Inser_Qty = new EFormsAnswer();
                    Inser_Qty.Name = "S" + (i.Separator.IsEmpty ? "" : "L");
                    Inser_Qty.NameEn = Inser_Qty.Name;
                    Inser_Qty.FillDate = DateTime.Now;
                    Inser_Qty.Type = i.Type;
                    Inser_Qty.Value = "";
                    Inser_Qty.ValueEn = "";
                    Inser_Qty.IndexOrder = i.IndexOrder;
                    Eform_Person.EFormsAnswers.Add(Inser_Qty);
                }
                else if (i.Type == "P")
                {
                    Inser_Qty = new EFormsAnswer();
                    Inser_Qty.Name = i.Paragraph.Name;
                    Inser_Qty.NameEn = i.Paragraph.Name;
                    Inser_Qty.FillDate = DateTime.Now;
                    Inser_Qty.Type = i.Type;
                    Inser_Qty.Value = "";
                    Inser_Qty.ValueEn = "";
                    Inser_Qty.IndexOrder = i.IndexOrder;
                    Eform_Person.EFormsAnswers.Add(Inser_Qty);
                }
                else if (i.Type == "T")
                {
                    Inser_Qty = new EFormsAnswer();
                    Inser_Qty.Name = i.LableName;
                    Inser_Qty.NameEn = i.LableNameEn;
                    Inser_Qty.FillDate = DateTime.Now;
                    Inser_Qty.Value = "";
                    Inser_Qty.ValueEn = "";
                    Inser_Qty.Type = i.Type;
                    Inser_Qty.IndexOrder = i.IndexOrder;
                    Eform_Person.EFormsAnswers.Add(Inser_Qty);
                }
                else if (i.Type == "G")
                {
                    //Inser_Qty = new EFormsAnswer();
                    Inser_Qty.Name = i.LableName;
                    Inser_Qty.NameEn = i.LableNameEn;
                    Inser_Qty.FillDate = DateTime.Now;
                    Inser_Qty.Value = "";
                    Inser_Qty.ValueEn = "";
                    Inser_Qty.Type = i.Type;
                    Inser_Qty.IndexOrder = i.IndexOrder;
                    Eform_Person.EFormsAnswers.Add(Inser_Qty);
                }
                else if (Inser_Qty != null || (Inser_Qty == null && !(i.Requird.HasValue && i.Requird.Value)))
                {
                    Inser_Qty.Name = i.LableName;
                    Inser_Qty.NameEn = i.LableNameEn;
                    Inser_Qty.FillDate = DateTime.Now;
                    Inser_Qty.Type = i.Type;
                    Inser_Qty.IndexOrder = i.IndexOrder;
                    Eform_Person.EFormsAnswers.Add(Inser_Qty);
                }
                //else
                //    throw new Exception("Ansqares");
            }
            Eform_Person.PreviewEformApprovals.Add(new PreviewEformApproval { OwnEform = true, Name = eform.UnitToApproveNavigation.UnitsNameAr, NameEn = eform.UnitToApproveNavigation.UnitsNameEn, UnitId = eform.UnitToApproveNavigation.UnitsId });

            await _appContext.PersonEforms.AddAsync(Eform_Person);
            await _appContext.SaveChangesAsync();

            return Ok(new ResponseClass() { Success = true, data = Eform_Person.Id });
        }
    }
}
