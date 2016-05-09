using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public class DashboardController : Controller
    {
        // TODO: Move this to s common class as its used in the Apprenticeship Controller as well
        public const string StandardsShortListCookieName = "standards_shortlist";

        private readonly IGetStandards _getStandards;
        private readonly IListCollection<int> _listCollection;

        public DashboardController(IGetStandards getStandards, IListCollection<int> listCollection)
        {
            _getStandards = getStandards;
            _listCollection = listCollection;
        }

        // GET: Dashboard
        public ActionResult Overview()
        {
            var shortListStandards = _listCollection.GetAllItems(StandardsShortListCookieName);
            var standards = shortListStandards.Select(x => _getStandards.GetStandardById(x));

            var shortlistStandardViewModels = standards.Select(x => new ShortlistStandardViewModel
            {
                Id = x.StandardId,
                Title = x.Title,
            });

            var viewModel = new DashboardViewModel
            {
                Title = "Your shortlisted apprenticeship training and training providers",
                Standards = shortlistStandardViewModels
            };

            return View(viewModel);
        }
    }
}