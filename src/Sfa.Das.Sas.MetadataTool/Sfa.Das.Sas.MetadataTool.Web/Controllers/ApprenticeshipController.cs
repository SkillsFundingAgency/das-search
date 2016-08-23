namespace Sfa.Das.Sas.MetadataTool.Web.Controllers
{
    using System.Web.Mvc;

    using Newtonsoft.Json;

    using ApplicationServices.Helpers;

    using ApplicationServices.Services.Interfaces;

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

        public ActionResult StandardDetails(int id)
        {
            var standard = _metaDataHelper.GetStandardMetaData(id);
            return this.View(standard);
        }

        public ActionResult FrameworkDetails(int id)
        {
            var framework = this._metaDataHelper.GetFrameworkMetaData(id);
            return this.View(framework);
        }
    }
}
