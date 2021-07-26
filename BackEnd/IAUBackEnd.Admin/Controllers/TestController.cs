using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Hosting;
using System.Web.Http;

namespace IAUBackEnd.Admin.Controllers
{
    public class TestController : ApiController
    {
		public IHttpActionResult Get()
		{
			return Ok(System.Web.Hosting.HostingEnvironment.MapPath("~") + "\\Log.txt");
		}
		public IHttpActionResult Create()
		{
			// Process the input somehow
			// ...

			Action<CancellationToken> workItem = PostToRemoteService;
			HostingEnvironment.QueueBackgroundWorkItem(workItem);

			return Ok();
		}

		private async void PostToRemoteService(CancellationToken cancellationToken)
		{
			using (var client = new HttpClient())
			{
				
			}
		}
	}
}
