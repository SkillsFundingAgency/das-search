using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
using Sfa.Das.Sas.Shared.Components.ViewComponents;
using Sfa.Das.Sas.Shared.Components.ViewComponents.Fat;
using Sfa.Das.Sas.Shared.Components.ViewModels;

namespace Sfa.Das.Sas.Shared.Components.Controllers
{
    public class FatController : Controller
    {
        private readonly IApprenticeshipSearchProvider _apprenticeshipSearchProvider;

        public FatController(IApprenticeshipSearchProvider apprenticeshipSearchProvider)
        {
            _apprenticeshipSearchProvider = apprenticeshipSearchProvider;
        }

        //[HttpPost]
        //[Route("Search")]
        public IActionResult Search(SearchQueryViewModel model)
        { 
            return View("Fat/SearchResults", model);
        }

        //public IActionResult Search(string keywords, int? page, int? size, int? sortOrder)
        //{
        //    var model = new SearchQueryViewModel()
        //    {
        //        Keywords = keywords
        //    };

        //    if (page != null)
        //    {
        //        model.Page = page.Value;
        //    }

        //    if (size != null)
        //    {
        //        model.ResultsToTake = size.Value;
        //    }

        //    if (sortOrder != null)
        //    {
        //        model.SortOrder = sortOrder.Value;
        //    }

        //    return Search(model);
        //}

    }
}
