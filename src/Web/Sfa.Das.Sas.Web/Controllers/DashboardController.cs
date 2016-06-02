using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.Factories;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IGetStandards _getStandards;
        private readonly IListCollection<int> _listCollection;
        private readonly IDashboardViewModelFactory _dashboardViewModelFactory;
        private readonly IShortlistStandardViewModelFactory _shortlistStandardViewModelFactory;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        public DashboardController(
            IGetStandards getStandards,
            IListCollection<int> listCollection,
            IDashboardViewModelFactory dashboardViewModelFactory,
            IShortlistStandardViewModelFactory shortlistStandardViewModelFactory,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository)
        {
            _getStandards = getStandards;
            _listCollection = listCollection;
            _dashboardViewModelFactory = dashboardViewModelFactory;
            _shortlistStandardViewModelFactory = shortlistStandardViewModelFactory;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
        }

        // GET: Dashboard
        public ActionResult Overview()
        {
            var shortListStandards = _listCollection.GetAllItems(Constants.StandardsShortListCookieName);

            var standards = new List<ShortlistStandardViewModel>();
            foreach (var shortlistedStandard in shortListStandards)
            {
                var standard = _getStandards.GetStandardById(shortlistedStandard.ApprenticeshipId);
                if (standard != null)
                {
                    var shortlistedStandardElement = _shortlistStandardViewModelFactory.GetShortlistStandardViewModel(standard.StandardId, standard.Title, standard.NotionalEndLevel);

                    var shortListedProviders = shortlistedStandard.ProvidersIdAndLocation
                                                       .Select(x => _apprenticeshipProviderRepository.GetCourseByStandardCode(
                                                           x.ProviderId,
                                                           x.LocationId.ToString(),
                                                           shortlistedStandard.ApprenticeshipId.ToString()));

                    var providerViewModels = shortListedProviders.Where(x => x != null)
                                                                 .Select(p => new ShortlistProviderViewModel
                                                                {
                                                                     Id = p.Provider.UkPrn,
                                                                     Name = p.Provider.Name,
                                                                     LocationId = p.Location.LocationId,
                                                                     Address = p.Location.Address
                                                                 });

                    shortlistedStandardElement.Providers.AddRange(providerViewModels);

                    standards.Add(shortlistedStandardElement);
                }
            }

            var viewModel = _dashboardViewModelFactory.GetDashboardViewModel(standards.ToList());

            return View(viewModel);
        }
    }
}