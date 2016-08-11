using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices.Helpers;

namespace MetadataTool.Controllers
{
    public class ApprenticeshipController : Controller
    {
        private readonly IMetaDataHelper _metaDataHelper;

        public ApprenticeshipController(IMetaDataHelper metaDataHelper)
        {
            _metaDataHelper = metaDataHelper;
        }

        public ActionResult Standards()
        {
            var standards = _metaDataHelper.GetAllStandardsMetaData();
            return View(standards);
        }

        public ActionResult Frameworks()
        {
            var frameworks = _metaDataHelper.GetAllFrameworkMetaData();
            return View(frameworks);
        }
    }
}
