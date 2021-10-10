using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Web.Http.Cors;

namespace IAU_BackEnd.Controllers
{
	public class Region_CityController : ApiController
	{
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public IHttpActionResult GetActive(int CountryID)
		{
			return Ok(new ResponseClass() { success = true, result = new { Regions = p.Region.Where(q => q.IS_Action.Value && q.Country_ID == CountryID), City = p.City.Where(q => q.Region.Country_ID == CountryID) } });
		}
	}
}
