﻿using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.ApplicationServices;
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
