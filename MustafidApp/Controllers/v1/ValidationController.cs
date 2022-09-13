using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MustafidApp.Helpers;
using MustafidAppDTO.DTO;
using MustafidAppModels.Context;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MustafidApp.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        public ValidationController(MustafidAppContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }
        /// <summary>
        /// Validate Request Type Page #2
        /// </summary>
        /// <param name="R_ID">Request Type ID</param>
        /// <param name="S_ID">Service Type ID</param>
        /// <returns>
        /// True: If Valid
        /// <br/>
        /// False: If Not
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> ValidateRequestType(int R_ID, int S_ID)
        {
            var data = await _appContext.RequestTypes.AnyAsync(q =>
                q.RequestTypeId == R_ID &&
                !q.Deleted &&
                q.IsAction.Value &&
                q.UnitsRequestTypes.Count(w =>
                    !w.Units.Deleted &&
                    (//check if unit service type "main and sub" contain service type
                        w.Units.ServiceTypeId == S_ID ||
                w.Units.UnitServiceTypes.Any(s =>
                            s.ServiceTypeId == S_ID
                        )
                    )
                    && w.Units.UnitMainServices.Count(r =>//check unit main service 
                        !r.MainService.Deleted &&
                        r.MainService.IsAction.Value &&
                        r.MainService.SubServices.Count(g =>
                            g.IsAction.Value &&
                            !g.Deleted
                            ) != 0
                        ) != 0

                ) != 0
            );

            return Ok(new ResponseClass() { Success = true, data = data });
        }
        /// <summary>
        /// Handel Personal Datat Validation
        /// </summary>
        /// <param name="Personal_data">PersonalDatat Obj</param>
        /// <remarks>
        ///     {
        ///      PD_APP_ID : Applicant Type ID,
        ///      PD_TitleNames_ID : نوع مقدم الطلب,
        ///      PD_F_Name:"",
        ///      PD_M_Name:"",
        ///      PD_L_Name:"",
        ///      PD_National_ID : الجنسية,
        ///      PD_ID_Doc_ID : نوع الهوية,
        ///      PD_ID_Number : رقم الهوية,
        ///      PD_C_ID : مكان الاقامة,
        ///      PD_Address_C_ID :  ID الدولة,
        ///      PD_Address_City_ID : ID المنطقة,
        ///      PD_Adress_R_ID : ID المدينة,
        ///      PD_Address_City : string : المنطقة,
        ///      PD_Adress_Region : string : المدينة,
        ///      PD_Postal : الرمز البريدي,
        ///      PD_mail :"" ,
        ///     }
        /// dd
        /// </remarks>
        /// <returns>
        /// <br/>
        /// NotValidCountry
        /// <br/>
        /// <br/>
        /// NotValidCityOrRegion
        /// <br/>
        /// NotValidRegion
        /// <br/>
        /// NotValidCity
        /// <br/>
        /// NotValidCityOrRegion
        /// <br/>
        /// NotValidTitleName
        /// <br/>
        /// NotValidNationality
        /// <br/>
        /// NotValidResidenceCountry
        /// <br/>
        /// NotValidApplicantID
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> ValidatePersonal(PersonalDataDTO Personal_data)
        {
            if (ModelState.IsValid)
            {
                #region Validate Region
                var ResidentCountry = await _appContext.Countries.FirstOrDefaultAsync(q => q.CountryId == Personal_data.PD_Address_C_ID && q.IsAction.Value);
                if (ResidentCountry == null)
                    return Ok(new ResponseClass { Success = false, data = "NotValidCountry" });

                if (ResidentCountry.CountryNameEn.ToLower().StartsWith("saudi"))
                {
                    if (Personal_data.PD_Address_City_ID == null || Personal_data.PD_Adress_R_ID == null)
                        return Ok(new ResponseClass { Success = false, data = "NotValidCityOrRegion" });


                    var Resident_Region = await _appContext.Regions.FirstOrDefaultAsync(q => q.IsAction.Value && q.RegionId == Personal_data.PD_Adress_R_ID && q.CountryId == Personal_data.PD_Address_C_ID);
                    if (Resident_Region == null)
                        return Ok(new ResponseClass { Success = false, data = "NotValidRegion" });

                    var Resident_City = await _appContext.Cities.FirstOrDefaultAsync(q => q.IsAction.Value && q.CityId == Personal_data.PD_Address_City_ID && q.RegionId == Personal_data.PD_Adress_R_ID);
                    if (Resident_City == null)
                        return Ok(new ResponseClass { Success = false, data = "NotValidCity" });

                }
                else
                {
                    if (Personal_data.PD_Address_City.Trim().IsNullOrEmpty() || Personal_data.PD_Adress_Region.Trim().IsNullOrEmpty())
                        return Ok(new ResponseClass { Success = false, data = "NotValidCityOrRegion" });
                }
                #endregion

                if (!(await _appContext.TitleMiddleNames.AnyAsync(q => q.TitleMiddleNamesId == Personal_data.PD_TitleNames_ID && q.IsAction.Value)))
                    return Ok(new ResponseClass { Success = false, data = "NotValidTitleName" });

                if (!(await _appContext.Countries.AnyAsync(q => q.CountryId == Personal_data.PD_National_ID && q.IsAction.Value)))
                    return Ok(new ResponseClass { Success = false, data = "NotValidNationality" });

                if (!(await _appContext.Countries.AnyAsync(q => q.CountryId == Personal_data.PD_C_ID && q.IsAction.Value)))
                    return Ok(new ResponseClass { Success = false, data = "NotValidResidenceCountry" });

                if (!(await _appContext.ApplicantTypes.AnyAsync(q => q.ApplicantTypeId == Personal_data.PD_APP_ID && q.IsAction.Value)))
                    return Ok(new ResponseClass { Success = false, data = "NotValidApplicantID" });

                return Ok(new ResponseClass { Success = true });

            }
            return Ok(new ResponseClass { Success = false, data = "There IS Requird Data is Not Valid|" + JsonConvert.SerializeObject(ModelState) });
        }

        /// <summary>
        /// Validate Page N#04
        /// </summary>
        /// <param name="R_ID">Request Type</param>
        /// <param name="S_ID">Service Type</param>
        /// <param name="APP_ID">Applicant Type</param>
        /// <param name="U_ID">Unit ID</param>
        /// <param name="M_ID">Main Service</param>
        /// <param name="SS_ID">Sub Service</param>
        /// <returns>
        /// NotValidMainServiceOrSubService
        /// <br/>
        /// NotValidUnit
        /// <br/>
        /// NotValidMainService
        /// <br/>
        /// NotValidSubService
        /// </returns>

        [HttpPost]
        public async Task<IActionResult> ValidateRequestData(int R_ID, int S_ID, int APP_ID, int U_ID, int? M_ID, int? SS_ID)
        {
            var req = await _appContext.RequestTypes.AnyAsync(q => q.RequestTypeId == R_ID && !q.Deleted && q.IsRequestType);

            if (req)
                if (!M_ID.HasValue || !SS_ID.HasValue)
                    return Ok(new ResponseClass { Success = false, data = "NotValidMainServiceOrSubService" });

            var Unit = await _appContext.Units.Where(q =>
                        q.UnitsId == U_ID &&
                        !q.Deleted &&
                        q.IsAction.Value &&
                        q.UnitMainServices.Count(w =>
                            w.MainService.ServiceTypeId == S_ID &&
                            !w.MainService.ServiceType.Deleted &&
                            w.MainService.IsAction.Value &&
                            !w.MainService.Deleted &&
                            (req ? true : w.MainService.SubServices.Count(t => t.IsAction.Value && !t.Deleted) != 0) &&//validated if inquiry then it's not required sub service
                            w.MainService.ValidTos.Count(e => e.ApplicantTypeId == APP_ID && !e.Deleted) != 0)//Check Applicant Type in main service
                        != 0 &&
                        (q.ServiceTypeId == S_ID || q.UnitServiceTypes.Count(d => d.ServiceTypeId == S_ID && !d.ServiceType.Deleted) != 0) && //check Service Type
                        q.UnitsRequestTypes.Any(w => w.RequestTypeId == R_ID && !w.RequestType.Deleted)//check request type
            ).FirstOrDefaultAsync();

            if (Unit == null)
                return Ok(new ResponseClass { Success = false, data = "NotValidUnit" });

            var mainServices = await _appContext.UnitMainServices.Where(q =>
                q.Unit.IsAction.Value &&
                !q.MainService.Deleted &&
                q.MainService.IsAction.Value &&
                q.MainService.ServiceTypeId == S_ID &&
                q.UnitId == U_ID &&
                !q.Unit.Deleted &&
                q.MainService.ValidTos.Any(w =>
                    w.ApplicantTypeId == APP_ID &&
                    !w.Deleted
                ) &&
                q.MainService.SubServices.Count(r =>
                    r.IsAction.Value &&
                    !r.Deleted
                ) != 0
            ).Select(q => q.MainService).FirstOrDefaultAsync(q => q.MainServicesId == M_ID);

            if (Unit == null)
                return Ok(new ResponseClass { Success = false, data = "NotValidMainService" });

            var subServices = await _appContext.SubServices.FirstOrDefaultAsync(q =>
                q.SubServicesId == SS_ID &&
                !q.Deleted &&
                q.IsAction.Value &&
                q.MainServicesId == M_ID &&
                !q.MainServices.Deleted
            );

            if (subServices == null)
                return Ok(new ResponseClass { Success = false, data = "NotValidSubService" });


            return Ok(new ResponseClass { Success = true });
        }
    }
}
