using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Collections;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public class DashboardController : Controller
    {
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
            var shortListStandards = _listCollection.GetAllItems(Constants.StandardsShortListCookieName);
            var standards = shortListStandards.Select(x => _getStandards.GetStandardById(x))
                                              .Where(s => s != null);
            
            var shortlistStandardViewModels = standards.Select(x => new ShortlistStandardViewModel
            {
                Id = x.StandardId,
                Title = x.Title,
                Level = x.NotionalEndLevel
            });

            var viewModel = new DashboardViewModel
            {
                Title = "Your apprenticeship shortlist",
                Standards = shortlistStandardViewModels
            };

            return View(viewModel);
        }
    }
}