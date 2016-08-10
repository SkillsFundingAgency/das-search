using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MetadataTool.Web.Controllers
{
    public class ApprenticeshipController : Controller
    {
        public ActionResult Standards()
        {
            return View();
        }

        public ActionResult Frameworks()
        {
            return View();
        }
    }
}