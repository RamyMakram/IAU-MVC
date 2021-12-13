using AdminPanel.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Home()
        {
            if (Request.Cookies["lang"] == null)
                Response.Cookies["lang"].Value = "ar";
            return View();
        }
        
        public ActionResult NotFound()
        {
            if (Request.Cookies["lang"] == null)
                Response.Cookies["lang"].Value = "ar";
            return View();
        }
        public ActionResult NotPermited()
        {
            if (Request.Cookies["lang"] == null)
                Response.Cookies["lang"].Value = "ar";
            return View();
        }
    }
}