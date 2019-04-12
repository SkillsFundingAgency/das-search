using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class ApprenticeshipDetailsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(ApprenticeshipDetailQueryViewModel searchQueryModel)
        {

            return View("Default");

        }
    }
}
