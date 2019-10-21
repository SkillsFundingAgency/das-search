using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.ViewComponents.TrainingProvider.Search;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class TrainingProviderController : Controller
    {
        public IActionResult Search(TrainingProviderSearchViewModel model)
        { 
            return View("TrainingProvider/SearchResults", model);
        }

        public IActionResult Details(TrainingProviderDetailQueryViewModel model)
        {
            return View("TrainingProvider/Details", model);
        }
 
    }
    
}
