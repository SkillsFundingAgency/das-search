using System;
using System.Threading.Tasks;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.ApplicationServices.Services;

namespace Sfa.Das.Sas.Shared.Components.Orchestrators
{
    public class FatOrchestrator : IFatOrchestrator
    {
        private readonly IApprenticeshipSearchService _apprenticeshipSearchService;
        private readonly IFatSearchResultsViewModelMapper _fatSearchResultsViewModelMapper;
        private readonly IFatSearchFilterViewModelMapper _fatSearchFilterViewModelMapper;
        private ICacheStorageService _cacheService;

        public FatOrchestrator(IApprenticeshipSearchService apprenticeshipSearchService, IFatSearchResultsViewModelMapper fatSearchResultsViewModelMapper, IFatSearchFilterViewModelMapper fatSearchFilterViewModelMapper, ICacheStorageService cacheService)
        {
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _fatSearchResultsViewModelMapper = fatSearchResultsViewModelMapper;
            _fatSearchFilterViewModelMapper = fatSearchFilterViewModelMapper;
            _cacheService = cacheService;
        }

        public async Task<FatSearchResultsViewModel> GetSearchResults(SearchQueryViewModel searchQueryModel)
        {

            
            var cacheKey = $"searchresults-{searchQueryModel.Keywords}-{searchQueryModel.Page}-{searchQueryModel.ResultsToTake}-{searchQueryModel.SortOrder}";

            foreach (var level in searchQueryModel.SelectedLevels)
            {
                cacheKey += $"-{level}";
            }

            var model = await _cacheService.RetrieveFromCache<FatSearchResultsViewModel>(cacheKey);

            if (model == null)
            {
                var results = _apprenticeshipSearchService.SearchByKeyword(searchQueryModel.Keywords, searchQueryModel.Page, searchQueryModel.ResultsToTake, searchQueryModel.SortOrder, searchQueryModel.SelectedLevels);

                model = _fatSearchResultsViewModelMapper.Map(await results);

                await _cacheService.SaveToCache(cacheKey, model, new TimeSpan(30, 0, 0, 0), new TimeSpan(1, 0, 0, 0));
            }
           
            return model;
        }

        public async Task<FatSearchFilterViewModel> GetSearchFilters(SearchQueryViewModel searchQueryModel)
        {
            var results = _apprenticeshipSearchService.SearchByKeyword(searchQueryModel.Keywords, searchQueryModel.Page, searchQueryModel.ResultsToTake, searchQueryModel.SortOrder, searchQueryModel.SelectedLevels);

            var model = _fatSearchFilterViewModelMapper.Map(await results, searchQueryModel);

            return model;
        }
    }
}