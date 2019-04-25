using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class FatController : Controller
    {
        public IActionResult Search(SearchQueryViewModel model)
        { 
            return View("Fat/SearchResults", model);
        }
        
        public IActionResult Apprenticeship(string id)
        {
            var model = new ApprenticeshipDetailQueryViewModel(){Id = id};
            return View("Fat/ApprenticeshipDetails", model);
        }

    }
}
