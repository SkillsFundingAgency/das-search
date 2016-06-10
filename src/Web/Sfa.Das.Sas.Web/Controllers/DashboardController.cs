using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Sfa.Das.Sas.Core.Domain.Services;
using Sfa.Das.Sas.Web.Collections;
using Sfa.Das.Sas.Web.Common;
using Sfa.Das.Sas.Web.ViewModels;

namespace Sfa.Das.Sas.Web.Controllers
{
    using Sfa.Das.Sas.Web.Factories.Interfaces;

    public class DashboardController : Controller
    {
        private readonly IGetStandards _getStandards;
        private readonly IGetFrameworks _getFrameworks;
        private readonly IListCollection<int> _listCollection;
        private readonly IDashboardViewModelFactory _dashboardViewModelFactory;
        private readonly IShortlistStandardViewModelFactory _shortlistStandardViewModelFactory;
        private readonly IShortlistFrameworkViewModelFactory _shortlistFrameworkViewModelFactory;
        private readonly IApprenticeshipProviderRepository _apprenticeshipProviderRepository;

        public DashboardController(
            IGetStandards getStandards,
            IGetFrameworks getFrameworks,
            IListCollection<int> listCollection,
            IDashboardViewModelFactory dashboardViewModelFactory,
            IShortlistStandardViewModelFactory shortlistStandardViewModelFactory,
            IShortlistFrameworkViewModelFactory shortlistFrameworkViewModelFactory,
            IApprenticeshipProviderRepository apprenticeshipProviderRepository)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _listCollection = listCollection;
            _dashboardViewModelFactory = dashboardViewModelFactory;
            _shortlistStandardViewModelFactory = shortlistStandardViewModelFactory;
            _shortlistFrameworkViewModelFactory = shortlistFrameworkViewModelFactory;
            _apprenticeshipProviderRepository = apprenticeshipProviderRepository;
        }

        // GET: Dashboard
        public ActionResult Overview()
        {
            var standards = GetShortlistedStandards();
            var frameworks = GetShortlistedFrameworks();

            var viewModel = _dashboardViewModelFactory.GetDashboardViewModel(standards, frameworks);

            return View(viewModel);
        }

        private ICollection<ShortlistStandardViewModel> GetShortlistedStandards()
        {
            var standardViewModels = new List<ShortlistStandardViewModel>();

            var shortlistedApprenticeships = _listCollection.GetAllItems(Constants.StandardsShortListCookieName);

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

                var standardViewModel = _shortlistStandardViewModelFactory.GetShortlistStandardViewModel(
                    standard.StandardId,
                    standard.Title,
                    standard.NotionalEndLevel);

                var shortListedProviders = apprenticeship.ProvidersIdAndLocation
                    .Select(x => _apprenticeshipProviderRepository.GetCourseByStandardCode(
                        x.ProviderId,
                        x.LocationId.ToString(),
                        apprenticeship.ApprenticeshipId.ToString()));

                var providerViewModels = shortListedProviders.Where(x => x != null)
                    .Select(p => new ShortlistProviderViewModel
                    {
                        Id = p.Provider.UkPrn,
                        Name = p.Provider.Name,
                        LocationId = p.Location.LocationId,
                        Address = p.Location.Address,
                        Url = Url.Action("Detail", "Provider", new { providerId = p.Provider.Id, locationId = p.Location.LocationId, standardCode = standard.StandardId })
                    });

                standardViewModel.Providers.AddRange(providerViewModels);

                standardViewModels.Add(standardViewModel);
            }

            return standardViewModels;
        }

        private ICollection<ShortlistFrameworkViewModel> GetShortlistedFrameworks()
        {
            var shortlistedApprenticeships = _listCollection.GetAllItems(Constants.FrameworksShortListCookieName);

            if (shortlistedApprenticeships == null)
            {
                return new List<ShortlistFrameworkViewModel>();
            }

            if (!shortlistedApprenticeships.Any())
            {
                return new List<ShortlistFrameworkViewModel>();
            }

            var frameworkIds = shortlistedApprenticeships.Select(x => x.ApprenticeshipId).ToList();

            if (!frameworkIds.Any())
            {
                return new List<ShortlistFrameworkViewModel>();
            }

            var frameworks = frameworkIds.Select(id => _getFrameworks.GetFrameworkById(id)).ToList();
            
            if (!frameworks.Any())
            {
                return new List<ShortlistFrameworkViewModel>();
            }

            var viewModels = frameworks.Select(f =>
                _shortlistFrameworkViewModelFactory.GetShortlistFrameworkViewModel(
                    f.FrameworkId,
                    f.Title,
                    f.Level));

            return viewModels.Where(vm => vm != null).ToList();
        }
    }
}