using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using IAU.DTO.Helper;
using IAU.DTO.Entity;

namespace IAU_BackEnd.Controllers.Applicant
{
    public class ApplicantTitleController : ApiController
    {
        private static MostafidDatabaseEntities p = new MostafidDatabaseEntities();

        [Route("GetTitles")]
        public async Task<IHttpActionResult> GetApplicantMiddleTitles(string lang)
        {
            try
            {

                var data = GetApplicantMiddleTitlesList(lang);
                return Ok(new ResponseClass
                {
                    success = true,
                    result = data
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    success = false
                });
            }
        }
        public static IEnumerable<SelectList_DTO> GetApplicantMiddleTitlesList(string lang)
        {
            try
            {
                var entity = p.Title_Middle_Names.Where(a => a.IS_Action == true).ToList()
                  .Select(a =>
                new SelectList_DTO
                {
                    ID = a.Title_Middle_Names_ID,
                    Name = (lang == "ar" ? a.Title_Middle_Names_Name_AR : a.Title_Middle_Names_Name_EN),
                });
                return entity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
