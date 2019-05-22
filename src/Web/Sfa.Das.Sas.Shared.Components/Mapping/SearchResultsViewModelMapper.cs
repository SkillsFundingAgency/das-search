using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using System.Linq;
using Sfa.Das.Sas.ApplicationServices.Responses;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class SearchResultsViewModelMapper : ISearchResultsViewModelMapper
    {
        private readonly ITrainingProviderSearchResultsItemViewModelMapper _providerSearchResultsItemMaper;

        public SearchResultsViewModelMapper(ITrainingProviderSearchResultsItemViewModelMapper providerSearchResultsItemMaper)
        {
            _providerSearchResultsItemMaper = providerSearchResultsItemMaper;
        }


        public SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel> Map(ProviderSearchResponse source, TrainingProviderSearchViewModel query)
        {
            var item = new SearchResultsViewModel<TrainingProviderSearchResultsItem, TrainingProviderSearchViewModel>()
            {
                SearchResults = source.Results.Hits.Select(s => _providerSearchResultsItemMaper.Map(s)),
                TotalResults = source.Results.TotalResults,
                LastPage = source.Results.LastPage,
                SearchQuery = query

            };
            return item;
        }
    }
}
