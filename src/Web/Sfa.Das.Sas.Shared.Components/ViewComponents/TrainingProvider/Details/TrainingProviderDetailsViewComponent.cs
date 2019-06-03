using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class TrainingProviderDetailsViewComponent : ViewComponent
    {
        private readonly ITrainingProviderOrchestrator _tpOrchestrator;

        public TrainingProviderDetailsViewComponent(ITrainingProviderOrchestrator tpOrchestrator)
        {
            _tpOrchestrator = tpOrchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync(TrainingProviderDetailQueryViewModel searchQueryModel, bool inline = false)
        {

            var model = await _tpOrchestrator.GetDetails(searchQueryModel);

            return View("../TrainingProvider/Details/Default", model);

        }
    }
}
