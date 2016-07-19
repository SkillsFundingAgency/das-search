using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.ApplicationServices.Settings;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Attribute;
using Sfa.Das.Sas.Web.Factories.Interfaces;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    [NoCache]
    public class DashboardController : Controller
    {
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly IShortlistCollection<int> _shortlistCollection;
        private readonly IDashboardViewModelFactory _dashboardViewModelFactory;
        private readonly IShortlistViewModelFactory _shortlistViewModelFactory;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        public DashboardController(
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            IShortlistCollection<int> shortlistCollection,
            IDashboardViewModelFactory dashboardViewModelFactory,
            IShortlistViewModelFactory shortlistViewModelFactory,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _shortlistCollection = shortlistCollection;
            _dashboardViewModelFactory = dashboardViewModelFactory;
            _shortlistViewModelFactory = shortlistViewModelFactory;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
        }

        // GET: Dashboard
        public ActionResult Overview()
        {
            var standards = GetShortlistedStandards();
            var frameworks = GetShortlistedFrameworks();
            var apprenticeships = standards.Concat(frameworks);

            var viewModel = _dashboardViewModelFactory.GetDashboardViewModel(apprenticeships);

            return View(viewModel);
        }

        private IEnumerable<IShortlistApprenticeshipViewModel> GetShortlistedStandards()
        {
            var standardViewModels = new List<IShortlistApprenticeshipViewModel>();

            var shortlistedApprenticeships = _shortlistCollection.GetAllItems(Constants.StandardsShortListName);

            if (shortlistedApprenticeships == null || !shortlistedApprenticeships.Any())
            {
                return standardViewModels;
            }

            foreach (var apprenticeship in shortlistedApprenticeships)
            {
                var standard = _getStandards.GetStandardById(apprenticeship.ApprenticeshipId);

                if (standard == null)
                {
                    continue;
                }

                var apprenticeshipViewModel = _shortlistViewModelFactory.GetShortlistViewModel(standard);

                var shortListedProviders = apprenticeship.ProvidersUkrpnAndLocation
                    .Select(x => _apprenticeshipProviderRepository.GetCourseByStandardCode(
                        x.Ukprn,
                        x.LocationId,
                        apprenticeship.ApprenticeshipId.ToString()));

                var providerViewModels = shortListedProviders.Where(x => x != null)
                    .Select(p => new ShortlistProviderViewModel
                    {
                        Ukprn = p.Provider.UkPrn,
                        Name = p.Provider.Name,
                        LocationId = p.Location.LocationId,
                        Address = p.Location.Address,
                        Url = Url.Action("Detail", "Provider", new { ukprn = p.Provider.UkPrn, locationId = p.Location.LocationId, standardCode = standard.StandardId })
                    });

                apprenticeshipViewModel.Providers.AddRange(providerViewModels);

                standardViewModels.Add(apprenticeshipViewModel);
            }

            return standardViewModels;
        }

        private IEnumerable<IShortlistApprenticeshipViewModel> GetShortlistedFrameworks()
        {
            var shortlistedApprenticeships = _shortlistCollection.GetAllItems(Constants.FrameworksShortListName);

            if (shortlistedApprenticeships == null)
            {
                return new List<IShortlistApprenticeshipViewModel>();
            }

            if (!shortlistedApprenticeships.Any())
            {
                return new List<IShortlistApprenticeshipViewModel>();
            }

            var frameworkViewModels = new List<IShortlistApprenticeshipViewModel>();

            foreach (var apprenticeship in shortlistedApprenticeships)
            {
                var framework = _getFrameworks.GetFrameworkById(apprenticeship.ApprenticeshipId);

                if (framework == null)
                {
                    continue;
                }

                var apprenticeshipViewModel = _shortlistViewModelFactory.GetShortlistViewModel(framework);

                var shortListedProviders = apprenticeship.ProvidersUkrpnAndLocation
                    .Select(x => _apprenticeshipProviderRepository.GetCourseByFrameworkId(
                        x.Ukprn,
                        x.LocationId,
                        apprenticeship.ApprenticeshipId.ToString()));

                var providerViewModels = shortListedProviders.Where(x => x != null)
                    .Select(p => new ShortlistProviderViewModel
                    {
                        Ukprn = p.Provider.UkPrn,
                        Name = p.Provider.Name,
                        LocationId = p.Location.LocationId,
                        Address = p.Location.Address,
                        Url = Url.Action("Detail", "Provider", new { ukprn = p.Provider.UkPrn, locationId = p.Location.LocationId, frameworkId = framework.FrameworkId })
                    });

                apprenticeshipViewModel.Providers.AddRange(providerViewModels);

                frameworkViewModels.Add(apprenticeshipViewModel);
            }

            return frameworkViewModels;
        }
    }
}