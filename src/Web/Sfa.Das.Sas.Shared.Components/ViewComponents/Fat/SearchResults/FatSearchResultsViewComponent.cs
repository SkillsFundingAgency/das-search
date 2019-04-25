using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class FatSearchResultsViewComponent : ViewComponent
    {
        private readonly ICssViewModel _cssViewModel;
        private readonly IApprenticeshipSearchService _apprenticeshipSearchService;
        private readonly IFatSearchResultsViewModelMapper _fatSearchResultsViewModelMapper;

        public FatSearchResultsViewComponent(ICssViewModel cssViewModel, IApprenticeshipSearchService apprenticeshipSearchService, IFatSearchResultsViewModelMapper fatSearchResultsViewModelMapper)
        {
            _cssViewModel = cssViewModel;
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _fatSearchResultsViewModelMapper = fatSearchResultsViewModelMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(SearchQueryViewModel searchQueryModel ,string cssModifier = null, bool inline = false)
        {
            if (cssModifier != null)
            {
                _cssViewModel.ClassModifier = cssModifier;
            }
            var results = _apprenticeshipSearchService.SearchByKeyword(searchQueryModel.Keywords, searchQueryModel.Page, searchQueryModel.ResultsToTake, searchQueryModel.SortOrder, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

            var model = _fatSearchResultsViewModelMapper.Map(results, _cssViewModel);
          
            return View("Default", model);

        }
    }
}
