using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.Mapping;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;
using Sfa.Das.Sas.Shared.Components.ViewModels.Css.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class TrainingProviderSearchResultsViewComponent : ViewComponent
    {
        private readonly ITrainingProviderOrchestrator _tpOrchestrator;

        public TrainingProviderSearchResultsViewComponent(ITrainingProviderOrchestrator tpOrchestrator)
        {
            _tpOrchestrator = tpOrchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync(TrainingProviderSearchViewModel searchQueryModel, bool inline = false)
        {

            var model = await _tpOrchestrator.GetSearchResults(searchQueryModel);

            return View("../TrainingProvider/SearchResults/Default", model);

        }
    }
}
