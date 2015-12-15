using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sfa.Eds.Das.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(SearchCriteria criteria)
        {
            return View();
        }
    }

    public class SearchCriteria
    {
        public string Keywords { get; set; }
    }
}