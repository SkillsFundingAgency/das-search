namespace Sfa.Das.Sas.MetadataTool.Web.Controllers
{
    using System.Web.Mvc;

    using Newtonsoft.Json;

    using ApplicationServices.Helpers;

    using ApplicationServices.Services.Interfaces;

    using Sfa.Das.Sas.Core.Models;

    public class ApprenticeshipController : Controller
    {
        private readonly IMetaDataHelper _metaDataHelper;

        private readonly IDocumentImporter _documentImporter;

        public ApprenticeshipController(
            IMetaDataHelper metaDataHelper,
            IDocumentImporter documentImporter)
        {
            _metaDataHelper = metaDataHelper;
            _documentImporter = documentImporter;
        }

        public ActionResult Standards()
        {
            var standards = _metaDataHelper.GetAllStandardsMetaData();
            return View(standards);
        }

        public ActionResult Frameworks()
        {
            var frameworks = _metaDataHelper.GetAllFrameworksMetaData();
            return View(frameworks);
        }

        public string StandardsJson()
        {
            var standards = _metaDataHelper.GetAllStandardsMetaData();
            return JsonConvert.SerializeObject(standards);
        }

        public string FrameworksJson()
        {
            var frameworks = _metaDataHelper.GetAllFrameworksMetaData();
            return JsonConvert.SerializeObject(frameworks);
        }

        public ActionResult StandardDetails(string id)
        {
            var standard = _metaDataHelper.GetStandardMetaData(id);
            return this.View(standard);
        }

        public ActionResult FrameworkDetails(string id)
        {
            var framework = _metaDataHelper.GetFrameworkMetaData(id);
            return View(framework);
        }

        public ActionResult EditStandard(string id)
        {
            var standard = _metaDataHelper.GetStandardMetaData(id);
            return View(standard);
        }

        public ActionResult EditFramework(string id)
        {
            var framework = _metaDataHelper.GetFrameworkMetaData(id);
            return View(framework);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateStandard(StandardMetaData model)
        {
            _metaDataHelper.UpdateStandardMetaData(model);

            return Redirect(Url.Action("StandardDetails", new { model.Id }));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateFramework(FrameworkMetaData model)
        {
            _metaDataHelper.UpdateFrameworkMetaData(model);
            return Redirect(Url.Action("FrameworkDetails",  new {id = model.Id }));
        }
    }
}
