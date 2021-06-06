using IAU.DTO.Helper;
using IAU_BackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace IAU_BackEnd.Controllers
{
    public class TitlesController : ApiController
    {
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public async Task<ICollection<Title_Middle_Names>> GetActive()
		{
			return p.Title_Middle_Names.Where(q => q.IS_Action.Value).ToList();
		}
	}
}
