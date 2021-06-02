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
    public class IDDOcController : ApiController
    {
		private MostafidDatabaseEntities p = new MostafidDatabaseEntities();
		public async Task<ICollection<ID_Document>> GetActive()
		{
			return p.ID_Document.Where(q => q.IS_Action.Value).ToList();
		}
	}
}
