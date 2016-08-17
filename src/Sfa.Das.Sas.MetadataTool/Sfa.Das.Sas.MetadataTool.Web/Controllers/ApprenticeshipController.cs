namespace Sfa.Das.Sas.MetadataTool.Web.Controllers
{
    using System.Web.Mvc;

    using Sfa.Das.Sas.ApplicationServices.Helpers;

    public class ApprenticeshipController : Controller
    {
        private readonly IMetaDataHelper _metaDataHelper;

        public ApprenticeshipController(IMetaDataHelper metaDataHelper)
        {
            this._metaDataHelper = metaDataHelper;
        }

        public ActionResult Standards()
        {
            var standards = this._metaDataHelper.GetAllStandardsMetaData();
            return this.View(standards);
        }

        public ActionResult Frameworks()
        {
            var frameworks = this._metaDataHelper.GetAllFrameworksMetaData();
            return this.View(frameworks);
        }

        public ActionResult StandardDetails(int id)
        {
            var standard = this._metaDataHelper.GetStandardMetaData(id);
            return this.View(standard);
        }

        public ActionResult FrameworkDetails(int id)
        {
            var framework = this._metaDataHelper.GetFrameworkMetaData(id);
            return this.View(framework);
        }
    }
}
