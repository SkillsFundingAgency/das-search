using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class FatSearchResultsViewComponent : ViewComponent
    {
        private readonly IFatOrchestrator _fatOrchestrator;

        public FatSearchResultsViewComponent(IFatOrchestrator fatOrchestrator)
        {
            _fatOrchestrator = fatOrchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync(SearchQueryViewModel searchQueryModel, bool inline = false)
        {

            var model = _fatOrchestrator.GetSearchResults(searchQueryModel);

            return View("Default", model);

        }
    }
}
