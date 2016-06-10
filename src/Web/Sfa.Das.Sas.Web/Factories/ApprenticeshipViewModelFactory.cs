namespace Sfa.Das.Sas.Web.Factories
{
    using System.Web.Mvc;

    using Sfa.Das.Sas.ApplicationServices.Models;
    using Sfa.Das.Sas.Core.Domain.Model;
    using Sfa.Das.Sas.Core.Domain.Services;
    using Sfa.Das.Sas.Web.Factories.Interfaces;
    using Sfa.Das.Sas.Web.Services;
    using Sfa.Das.Sas.Web.ViewModels;

    public class ApprenticeshipViewModelFactory : IApprenticeshipViewModelFactory
    {
        private readonly IGetStandards _getStandards;

        private readonly IGetFrameworks _getFrameworks;

        private readonly IMappingService _mappingService;

        public ApprenticeshipViewModelFactory(IGetStandards getStandards, IGetFrameworks getFrameworks, IMappingService mappingService)
        {
            _getStandards = getStandards;
            _getFrameworks = getFrameworks;
            _mappingService = mappingService;
        }

        public ProviderSearchViewModel GetProviderSearchViewModelForStandard(int id, UrlHelper urlHelper)
        {
            var standardResult = _getStandards.GetStandardById(id);
            var viewModel = CreateViewModel(standardResult.StandardId, standardResult.NotionalEndLevel, standardResult.Title);

            viewModel.PostUrl = urlHelper.Action("StandardResults", "Provider");
            viewModel.PreviousPageLink = new LinkViewModel
                                             {
                                                 Title = "Go back to apprenticeship",
                                                 Url = urlHelper.Action("Standard", "Apprenticeship", new { Id = id })
                                             };
            return viewModel;
        }

        public ProviderSearchViewModel GetFrameworkProvidersViewModel(int id, UrlHelper urlHelper)
        {
            var fwResult = _getFrameworks.GetFrameworkById(id);
            var viewModel = CreateViewModel(fwResult.FrameworkId, fwResult.Level, fwResult.Title);

            viewModel.PostUrl = urlHelper.Action("FrameworkResults", "Provider");
            viewModel.PreviousPageLink = new LinkViewModel
                                             {
                                                 Title = "Go back to apprenticeship",
                                                 Url = urlHelper.Action("Framework", "Apprenticeship", new { Id = id })
                                             };

            return viewModel;
        }

        public StandardViewModel GetStandardViewModel(int id)
        {
            var standardResult = _getStandards.GetStandardById(id);
            if (standardResult == null)
            {
                return null;
            }

            return _mappingService.Map<Standard, StandardViewModel>(standardResult);
        }

        public FrameworkViewModel GetFrameworkViewModel(int id)
        {
            var frameworkResult = _getFrameworks.GetFrameworkById(id);
            if (frameworkResult == null)
            {
                return null;
            }

            return _mappingService.Map<Framework, FrameworkViewModel>(frameworkResult);
        }

        public ApprenticeshipSearchResultViewModel GetSApprenticeshipSearchResultViewModel(ApprenticeshipSearchResults searchResults)
        {
            return _mappingService.Map<ApprenticeshipSearchResults, ApprenticeshipSearchResultViewModel>(searchResults);
        }

        private ProviderSearchViewModel CreateViewModel(int id, int level, string title)
        {
            var viewModel = new ProviderSearchViewModel
            {
                Title = title + ", level " + level,
                ApprenticeshipId = id
            };

            return viewModel;
        }
    }
}