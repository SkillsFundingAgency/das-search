using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices.Models;
using Sfa.Das.Sas.Shared.Components.Orchestrators;
using Sfa.Das.Sas.Shared.Components.ViewComponents.ApprenticeshipDetails;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat.SearchResults
{
    public class TrainingProviderContactDetailsViewComponent : ViewComponent
    {
        private readonly ITrainingProviderOrchestrator _tpOrchestrator;
        private readonly IApprenticeshipOrchestrator _apprenticeshipOrchestrator;

        public TrainingProviderContactDetailsViewComponent(ITrainingProviderOrchestrator tpOrchestrator, IApprenticeshipOrchestrator apprenticeshipOrchestrator)
        {
            _tpOrchestrator = tpOrchestrator;
            _apprenticeshipOrchestrator = apprenticeshipOrchestrator;
        }

        public async Task<IViewComponentResult> InvokeAsync(TrainingProviderDetailQueryViewModel searchQueryModel, bool inline = false)
        {

            searchQueryModel.ApprenticeshipType = _apprenticeshipOrchestrator.GetApprenticeshipType(searchQueryModel.ApprenticeshipId) ;
            var model = await _tpOrchestrator.GetDetails(searchQueryModel);

            model.SearchQuery = searchQueryModel;

            if (searchQueryModel.ApprenticeshipType == ApprenticeshipType.Standard)
            {
                model.Apprenticeship = await _apprenticeshipOrchestrator.GetStandard(searchQueryModel.ApprenticeshipId);
            }
            else
            {
                model.Apprenticeship = await _apprenticeshipOrchestrator.GetFramework(searchQueryModel.ApprenticeshipId);
            }
         

            return View("../TrainingProvider/ContactDetails/Default", model);

        }
    }
}
