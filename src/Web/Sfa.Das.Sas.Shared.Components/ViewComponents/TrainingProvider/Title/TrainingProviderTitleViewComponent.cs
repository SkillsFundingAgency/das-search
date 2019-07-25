using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class TrainingProviderTitleViewComponent : ViewComponent
    {
        private readonly ITrainingProviderOrchestrator _tpOrchestrator;
        private readonly IApprenticeshipOrchestrator _apprenticeshipOrchestrator;

        public TrainingProviderTitleViewComponent(ITrainingProviderOrchestrator tpOrchestrator, IApprenticeshipOrchestrator apprenticeshipOrchestrator)
        {
            _tpOrchestrator = tpOrchestrator;
            _apprenticeshipOrchestrator = apprenticeshipOrchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync(TrainingProviderDetailQueryViewModel searchQueryModel, bool inline = false)
        {

            searchQueryModel.ApprenticeshipType = _apprenticeshipOrchestrator.GetApprenticeshipType(searchQueryModel.ApprenticeshipId) ;
            var model = await _tpOrchestrator.GetDetails(searchQueryModel);

            model.SearchQuery = searchQueryModel;

           
            return View("../TrainingProvider/Title/Default", model.Name);

        }
    }
}
