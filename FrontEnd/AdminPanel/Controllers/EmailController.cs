using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
	public class EmailController : BaseController
	{
		// GET: Email
		public ActionResult Home()
		{
			return View();
		}
		public ActionResult Preview(int id)
		{
			return View();
		}
	}
}