using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.Mapping;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class FatSearchResultsViewComponent : ViewComponent
    {
        private readonly ICssClasses _cssClasses;
        private readonly IApprenticeshipSearchService _apprenticeshipSearchService;
        private readonly IFatSearchResultsViewModelMapper _fatSearchResultsViewModelMapper;

        public FatSearchResultsViewComponent(ICssClasses cssClasses, IApprenticeshipSearchService apprenticeshipSearchService, IFatSearchResultsViewModelMapper fatSearchResultsViewModelMapper)
        {
            _cssClasses = cssClasses;
            _apprenticeshipSearchService = apprenticeshipSearchService;
            _fatSearchResultsViewModelMapper = fatSearchResultsViewModelMapper;
        }

        public async Task<IViewComponentResult> InvokeAsync(string keywords,string cssModifier = null, bool inline = false)
        {
            if (cssModifier != null)
            {
                _cssClasses.ClassModifier = cssModifier;
            }
            var results = _apprenticeshipSearchService.SearchByKeyword(keywords, 1, 20, 0, new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8 });

            var model = _fatSearchResultsViewModelMapper.Map(results, _cssClasses);
          
            return View("Default", model);

        }
    }
}
