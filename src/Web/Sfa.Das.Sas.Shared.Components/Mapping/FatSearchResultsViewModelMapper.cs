using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using System.Linq;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class FatSearchResultsViewModelMapper : IFatSearchResultsViewModelMapper
    {
        private IFatSearchResultsItemViewModelMapper _fatSearchResultsItemViewModelMapper;

        public FatSearchResultsViewModelMapper(IFatSearchResultsItemViewModelMapper fatSearchResultsItemViewModelMapper)
        {
            _fatSearchResultsItemViewModelMapper = fatSearchResultsItemViewModelMapper;
        }

        public FatSearchResultsViewModel Map(ApprenticeshipSearchResults source)
        {
            var item = new FatSearchResultsViewModel()
            {
                SearchResults = source.Results?.Select(s => _fatSearchResultsItemViewModelMapper.Map(s)),
                TotalResults = source.TotalResults,
                LastPage = source.LastPage,
                SearchQuery = {
                    
                    Page = source.ActualPage,
                    ResultsToTake = source.ResultsToTake,
                    SortOrder = !string.IsNullOrWhiteSpace(source.SortOrder) ?  int.Parse(source.SortOrder) : 0,
                    Keywords = source.SearchTerm,
                    SelectedLevels = source.SelectedLevels?.ToList()
                }

            };
            return item;
        }
    }
}
