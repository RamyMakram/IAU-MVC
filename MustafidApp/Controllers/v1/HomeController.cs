using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MustafidApp.Helpers;
using MustafidAppDTO.DTO;
using MustafidAppModels.Context;
using MustafidAppModels.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MustafidApp.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private MustafidAppContext _appContext;
        private IMapper _mapper;
        public HomeController(MustafidAppContext appContext, IMapper mapper)
        {
            _appContext = appContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Returns All Valid Service Type
        /// </summary>
        /// <returns>
        /// List of Service Type Object
        /// </returns>
        /// 
        [HttpGet]
        public async Task<IActionResult> GetServiceType()
        {
            var data = await _appContext.ValidTos.Where(q =>
                !q.Deleted &&
                q.MainService.IsAction.Value &&
                q.MainService.ServiceType.IsAction.Value &&
                !q.MainService.ServiceType.Deleted &&
                q.ApplicantType.IsAction.Value &&
                q.MainService.UnitMainServices.Count(s =>
                    s.Unit.IsAction.Value &&
                    !s.Unit.Deleted &&
                    !s.MainService.Deleted &&
                    s.MainService.IsAction.Value &&
                    s.MainService.SubServices.Count(t =>
                        t.IsAction.Value &&
                        !t.Deleted
                        ) != 0
                    ) != 0
            ).Select(q => q.MainService.ServiceType).Distinct().ToListAsync();

            var data_DTO = _mapper.Map<List<ServiceTypeDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }


        /// <summary>
        /// Returns All Valid Request Type base on given Service Type ID
        /// </summary>
        /// <param name="S_ID">S_ID is Service Type ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRequestType(int S_ID)
        {
            var data = await _appContext.RequestTypes.Where(q =>
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
            ).ToListAsync();

            var data_DTO = _mapper.Map<List<RequestTypeDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Returns All Valid Applicant Type base on given Service Type ID and Request Type ID "نوع مقدم الطلب"
        /// </summary>
        /// <param name="S_ID">S_ID is Service Type ID that User Choose It</param>
        /// <param name="R_ID">R_ID is Request Type ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAAPType(int S_ID, int R_ID)
        {
            var req = await _appContext.RequestTypes.AnyAsync(q => q.RequestTypeId == R_ID && !q.Deleted && q.RequestTypeNameEn.ToLower().Contains("inq"));
            var data = await _appContext.ValidTos.Where(q =>
            !q.Deleted &&
            !q.MainService.Deleted &&
            q.MainService.IsAction.Value &&
            q.MainService.ServiceTypeId == S_ID &&//get main service
            q.ApplicantType.IsAction.Value &&
            (req ? true : q.MainService.SubServices.Any(g => g.IsAction.Value && !g.Deleted)) &&//check inquiry for sub service
            q.MainService.UnitMainServices.Count(w =>//check if main service exist in unit main service
                w.Unit.IsAction.Value &&//check if unit is active
                !w.Unit.Deleted &&
                w.Unit.UnitsRequestTypes.Count(s =>//check request type
                    s.RequestTypeId == R_ID
                    ) != 0
                ) != 0
            ).Select(q => q.ApplicantType).Distinct().OrderBy(q => q.Index).ToListAsync();

            var data_DTO = _mapper.Map<List<ApplicantTypeDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Returns All Valid Title Names Like Mr,Mrs,Dr,Eng "اللقب"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTitlesNames()
        {
            var data = await _appContext.TitleMiddleNames.Where(q => q.IsAction.Value).ToListAsync();

            var data_DTO = _mapper.Map<List<TitleNamesDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Returns All Valid Countres Like SaudiArabia,Egypt "الجنسية و مكان الاقامة و الدولة"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCountry()
        {

            var data = await _appContext.Countries.Where(q => q.IsAction.Value).OrderBy(q => q.Index).ToListAsync();

            var data_DTO = _mapper.Map<List<CountryDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Returns All Valid ID Docs in "نوع الهوية"
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetIdentifactionsNames()
        {

            var data = await _appContext.IdDocuments.Where(q => q.IsAction.Value).ToListAsync();

            var data_DTO = _mapper.Map<List<IDDocsDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Return Valid Regions And Cities
        /// if Return Empty List You Must Transfare Dropdown to TXT
        /// </summary>
        /// <param name="C_ID">C_ID is Country ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetCitiesAndRegions(int C_ID)
        {
            var data = await _appContext.Regions.Include(q => q.Cities).Where(q => q.IsAction.Value && q.CountryId == C_ID).ToListAsync();

            var data_DTO = _mapper.Map<List<RegionDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Return Valid Units for "حدد الخدمة"
        /// </summary>
        /// <param name="S_ID">S_ID is Service Type ID that User Choose It</param>
        /// <param name="R_ID">S_ID is Request Type ID that User Choose It</param>
        /// <param name="APP_ID">APP_ID is Applicant Type ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUnitService(int R_ID, int S_ID, int APP_ID)
        {
            var ReqType = await _appContext.RequestTypes.FirstOrDefaultAsync(q => q.RequestTypeId == R_ID && !q.Deleted);
            var req = ReqType.RequestTypeNameEn.ToLower().Contains("inq");

            var data = await _appContext.Units.Where(q =>
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
            )
               .OrderBy(q => q.LevelId).ToListAsync();

            var data_DTO = _mapper.Map<List<UnitsDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Return Valid MainServices "الخدمة الرئيسية"
        /// </summary>
        /// <param name="U_ID">U_ID is Unit ID that User Choose It</param>
        /// <param name="S_ID">S_ID is Service Type ID that User Choose It</param>
        /// <param name="APP_ID">APP_ID is Applicant Type ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMServices(int U_ID, int S_ID, int APP_ID)//Call if inquiry
        {
            var data = await _appContext.UnitMainServices.Where(q =>
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
            ).Select(q => q.MainService).ToListAsync();

            var data_DTO = _mapper.Map<List<MainSerivceDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Return Valid SubServices "الخدمة الفرعية"
        /// </summary>
        /// <param name="M_ID">M_ID is Main Service ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetSServices(int M_ID)
        {
            var data = await _appContext.SubServices.Include(q => q.RequiredDocuments).Where(q =>
                !q.Deleted &&
                q.IsAction.Value &&
                q.MainServicesId == M_ID &&
                !q.MainServices.Deleted
            ).ToListAsync();

            var data_DTO = _mapper.Map<List<SubSerivceDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }
        /// <summary>
        /// Return Eforms Names
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// SS_ID=549,632,661
        /// <br /> 
        /// <param name="SS_ID">SS_ID is Sub Service ID that User Choose It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEforms(int SS_ID)
        {
            var data = await _appContext.EForms
                .Where(q =>
                    !q.Deleted &&
                    q.IsAction.Value &&
                    q.SubServiceId == SS_ID
            ).ToListAsync();

            var data_DTO = _mapper.Map<List<EFormDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }

        /// <summary>
        /// Return Eform Detailed
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// SS_ID=549,632,661
        /// <br /> 
        /// Q_Type property is 
        /// <br /> 
        /// I: Input Type
        /// <br /> 
        /// D: Input Type With Date Prop
        /// <br /> 
        /// R: Radio Type
        /// <br /> 
        /// C: Check Box
        /// <br /> 
        /// P: Paragraph
        /// <br /> 
        /// S: Separtor
        /// <br /> 
        /// T: Title ('user ar and end in quest type')
        /// <br /> 
        /// E: Refer To Exist Field
        /// <br /> 
        /// G: Grid use ('nrows,table columns')
        /// <br /> 
        /// </remarks>
        /// <param name="SS_ID">SS_ID is Sub Service ID that User Choose It</param>
        /// <param name="EF_ID">EF_ID is Eforms ID that User Will Fill It</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEformDetails(int SS_ID,int EF_ID)
        {
            var data = await _appContext.EForms.Where(q=>q.Id==EF_ID)
                .Include(q => q.UnitToApproveNavigation)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Paragraph)
                .Include(q => q.Questions)
                .ThenInclude(q => q.Separator)
                .Include(q => q.Questions)
                .ThenInclude(q => q.CheckBoxTypes)
                .Include(q => q.Questions)
                .ThenInclude(q => q.RadioTypes)
                .Include(q => q.Questions)
                .ThenInclude(q => q.TableColumns)
                .Where(q =>
                    !q.Deleted &&
                    q.IsAction.Value &&
                    q.SubServiceId == SS_ID
            ).ToListAsync();

            var data_DTO = _mapper.Map<List<EFormDTO>>(data);

            return Ok(new ResponseClass() { Success = true, data = data_DTO });
        }
    }
}
