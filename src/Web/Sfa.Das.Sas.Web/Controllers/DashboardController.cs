using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Factories;

namespace Sfa.Das.Sas.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IGetStandards _getStandards;
        private readonly IListCollection<int> _listCollection;
        private readonly IDashboardViewModelFactory _dashboardViewModelFactory;
        private readonly IShortlistStandardViewModelFactory _shortlistStandardViewModelFactory;

        public DashboardController(
            IGetStandards getStandards,
            IListCollection<int> listCollection,
            IDashboardViewModelFactory dashboardViewModelFactory,
            IShortlistStandardViewModelFactory shortlistStandardViewModelFactory)
        {
            _getStandards = getStandards;
            _listCollection = listCollection;
            _dashboardViewModelFactory = dashboardViewModelFactory;
            _shortlistStandardViewModelFactory = shortlistStandardViewModelFactory;
        }

        // GET: Dashboard
        public ActionResult Overview()
        {
            var shortListStandards = _listCollection.GetAllItems(Constants.StandardsShortListCookieName);

            var standards = _getStandards.GetStandardsByIds(shortListStandards);

            var shortlistStandardViewModels = standards.Select(
                x => _shortlistStandardViewModelFactory.GetShortlistStandardViewModel(
                    x.StandardId,
                    x.Title,
                    x.NotionalEndLevel));

            var viewModel = _dashboardViewModelFactory.GetDashboardViewModel(shortlistStandardViewModels.ToList());

            return View(viewModel);
        }
    }
}