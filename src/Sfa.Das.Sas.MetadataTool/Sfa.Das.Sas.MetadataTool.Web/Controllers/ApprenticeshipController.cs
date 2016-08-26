namespace Sfa.Das.Sas.MetadataTool.Web.Controllers
{
    using System;
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
        public ActionResult UpdateStandard(StandardMetaData model)
        {
            _metaDataHelper.UpdateStandardMetaData(model);
            var standard = _metaDataHelper.GetStandardMetaData(model.Id.ToString());
            return View("StandardDetails", standard);
        }

        [HttpPost]
        public ActionResult UpdateFramework(FrameworkMetaData model)
        {
            _metaDataHelper.UpdateFrameworkMetaData(model);
            var framework = _metaDataHelper.GetFrameworkMetaData(model.Id.ToString());
            return View("FrameworkDetails", framework);
        }
    }

    public class UpdateFrameworkModel
    {
        public Guid Id { get; set; }

        public string FrameworkOverview { get; set; }

        public string EntryRequirements { get; set; }

        public string ProfessionalRegistration { get; set; }
    }
}
