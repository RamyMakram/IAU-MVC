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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef_data">Data Of Eform</param>
        /// <param name="EF_ID">Eform ID</param>
        /// <remarks>
        ///      [
        ///              {
        ///                  "EFAns_Q_ID": رقم السؤال,
        ///                  "EFAns_EF_ID": رقم النموذج,
        ///                  "EFAns_Value": لو radio or check بيبق JSON,
        ///                  "EFAns_Value_EN": لو radio or check بيبق JSON,
        ///                  "EFAns_TableCol": لو جدول [
        ///                      {
        ///                          "TC_ID":  رقم ال column,
        ///                          "TC_Answare": [
        ///                              {
        ///                                  "TAns_Row":رقم ال row,
        ///                                  "TAns_Val": ""
        ///                              },
        ///                              {
        ///                                  "TAns_Row": رقم ال row,
        ///                                  "TAns_Val": ""
        ///                              }
        ///                          ]
        ///                      }
        ///                  ]
        ///              },
        ///              {
        ///                  "EFAns_Q_ID": رقم السؤال,
        ///                  "EFAns_EF_ID": رقم النموذج,
        ///                  "EFAns_Value": لو radio or check بيبق JSON,
        ///                  "EFAns_Value_EN": لو radio or check بيبق JSON,
        ///              }
        ///          ]
        ///dd
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveEfomr(IList<EformAnsDTO> ef_data, int EF_ID/*, bool IsUpdate, int? OldEFID*/)
        {
            var eform = await _appContext.EForms
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Separator)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Paragraph)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.Paragraph)
                .Include(q => q.Questions)
                    .ThenInclude(q => q.TableColumns)
                .Include(q => q.UnitToApproveNavigation)
                .FirstOrDefaultAsync(q => q.Id == EF_ID && !q.Deleted && q.IsAction.Value);
            if (eform == null)
                return Ok(new ResponseClass() { Success = false, data = "" });

            //if (IsUpdate && OldEFID.HasValue)
            //{
            //    _appContext.PersonEforms.Remove(_appContext.PersonEforms.FirstOrDefault(q => q.Id == OldEFID));
            //    await _appContext.SaveChangesAsync();
            //}

            var EF_Model = _mapper.Map<IList<EFormsAnswer>>(ef_data);

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

                    var EF_DataDTO = ef_data.FirstOrDefault(q => q.EFAns_Q_ID == i.Id);
                    var Index = 0;
                    foreach (var Col in EF_DataDTO.EFAns_TableCol)
                    {
                        var RearCol = i.TableColumns.FirstOrDefault(q => q.Id == Col.TC_ID);
                        
                        var SavedCols = Inser_Qty.PreviewTableCols.ElementAt(Index);
                        
                        SavedCols.Name = RearCol.Name;
                        SavedCols.NameEn = RearCol.NameEn;
                        
                        Index++;
                    }

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
