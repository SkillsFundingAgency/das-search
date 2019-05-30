using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class TrainingProviderController : Controller
    {
        public IActionResult Search(TrainingProviderSearchViewModel model)
        { 
            return View("TrainingProvider/SearchResults", model);
        }
        
    }
}
