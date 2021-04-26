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

namespace IAU_BackEnd.Controllers.BasicData
{
	public class CountryController : ApiController
	{
		private static MostafidDatabaseEntities p = new MostafidDatabaseEntities();

		[Route("GetAll")]
		public async Task<IHttpActionResult> GetAllCountries(string lang)
		{
			try
			{
				var entity = GetCountriesList(lang);
				return Ok(new ResponseClass
				{
					success = true,
					result = entity
				});
			}
			catch (Exception ex)
			{
				return Ok(new
				{
					success = false
				});
			}
		}

		public static IEnumerable<SelectList_DTO> GetCountriesList(string lang)
		{
			try
			{

				var entity = p.Country.Where(a => a.IS_Action == true).ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.Country_ID,
					Name = (lang == "ar" ? a.Country_Name_AR : a.Country_Name_EN),

				});
				return entity;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static IEnumerable<SelectList_DTO> GetRegionsList(string lang)
		{
			try
			{
				var entity = p.Region.Where(a => a.IS_Action == true).ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.Region_ID,
					Name = (lang == "ar" ? a.Region_Name_AR : a.Region_Name_EN),

				});
				return entity;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static IEnumerable<SelectList_DTO> GetCitiesList(string lang)
		{
			try
			{
				var entity = p.City.Where(a => a.IS_Action == true).ToList()
				  .Select(a =>
				new SelectList_DTO
				{
					ID = a.City_ID,
					Name = (lang == "ar" ? a.City_Name_AR : a.City_Name_EN),
					SubID = a.Region_ID

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
