using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using System.Linq;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.Mapping
{
    public class FatSearchResultsViewModelMapper : IFatSearchResultsViewModelMapper
    {
        private IFatSearchResultsItemViewModelMapper _fatSearchResultsItemViewModelMapper;

        public FatSearchResultsViewModelMapper(IFatSearchResultsItemViewModelMapper fatSearchResultsItemViewModelMapper)
        {
            _fatSearchResultsItemViewModelMapper = fatSearchResultsItemViewModelMapper;
        }

        public FatSearchResultsViewModel Map(ApprenticeshipSearchResults source, ICssClasses cssClasses)
        {
            var item = new FatSearchResultsViewModel()
            {
                CssClasses = cssClasses,
                SearchResults = source.Results.Select(s => _fatSearchResultsItemViewModelMapper.Map(s,cssClasses)),
                TotalResults = source.TotalResults,
                CurrentPage = source.ActualPage,
                LastPage = source.LastPage,
                ResultsToTake = source.ResultsToTake,
                SortOrder = source.SortOrder
            };
            return item;
        }
    }
}
