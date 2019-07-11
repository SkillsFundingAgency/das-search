using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class FatOrchestrator : IFatOrchestrator
    {
        private readonly IApprenticeshipSearchService _apprenticeshipSearchService;
        private readonly IFatSearchResultsViewModelMapper _fatSearchResultsViewModelMapper;
        private readonly IFatSearchFilterViewModelMapper _fatSearchFilterViewModelMapper;

        public FatOrchestrator(IApprenticeshipSearchService apprenticeshipSearchService, IFatSearchResultsViewModelMapper fatSearchResultsViewModelMapper, IFatSearchFilterViewModelMapper fatSearchFilterViewModelMapper)
        {
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _fatSearchResultsViewModelMapper = fatSearchResultsViewModelMapper;
            _fatSearchFilterViewModelMapper = fatSearchFilterViewModelMapper;
        }

        public FatSearchResultsViewModel GetSearchResults(SearchQueryViewModel searchQueryModel)
        {
            var results = _apprenticeshipSearchService.SearchByKeyword(searchQueryModel.Keywords, searchQueryModel.Page, searchQueryModel.ResultsToTake, searchQueryModel.SortOrder, searchQueryModel.SelectedLevels);

            var model = _fatSearchResultsViewModelMapper.Map(results);

            return model;
        }

        public FatSearchFilterViewModel GetSearchFilters(SearchQueryViewModel searchQueryModel)
        {
            var results = _apprenticeshipSearchService.SearchByKeyword(searchQueryModel.Keywords, searchQueryModel.Page, searchQueryModel.ResultsToTake, searchQueryModel.SortOrder, searchQueryModel.SelectedLevels);

            var model = _fatSearchFilterViewModelMapper.Map(results, searchQueryModel);

            return model;
        }
    }
}