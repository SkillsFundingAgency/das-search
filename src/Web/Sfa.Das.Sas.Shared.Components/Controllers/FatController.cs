using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.ViewComponents;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class FatController : Controller
    {
        private readonly IApprenticeshipSearchProvider _apprenticeshipSearchProvider;

        public FatController(IApprenticeshipSearchProvider apprenticeshipSearchProvider)
        {
            _apprenticeshipSearchProvider = apprenticeshipSearchProvider;
        }

        [HttpPost]
        public IActionResult Search(FatSearchViewModel model)
        {
            
            return View("Fat/SearchResults", model);
        }

        public IActionResult Search(string keywords)
        {
            return Search(new FatSearchViewModel() {Keywords = keywords});
        }

    }
}
