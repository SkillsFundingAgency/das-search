using Microsoft.AspNetCore.Mvc;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Basket
{
    public class AddToBasketViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string apprenticeshipId)
        {
            var model = new AddToBasketViewModel { ApprenticeshipId = apprenticeshipId };

            return View("Default", model);
        }
    }
}
