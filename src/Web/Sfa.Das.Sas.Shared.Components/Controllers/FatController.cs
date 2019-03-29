using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.ViewComponents;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    [Route("FindApprenticeshipTraining")]
    public class FatController : Controller
    {
        private readonly IApprenticeshipSearchProvider _apprenticeshipSearchProvider;

        public FatController(IApprenticeshipSearchProvider apprenticeshipSearchProvider)
        {
            _apprenticeshipSearchProvider = apprenticeshipSearchProvider;
        }

        public IActionResult Search(FatSearchViewModel model)
        {

           var results = _apprenticeshipSearchProvider.SearchByKeyword(model.Keywords,1,20,0, new List<int>(){0,1,2,3,4,5,6,7,8});
            return View("Fat/SearchResults",results);
        }


    }
}
