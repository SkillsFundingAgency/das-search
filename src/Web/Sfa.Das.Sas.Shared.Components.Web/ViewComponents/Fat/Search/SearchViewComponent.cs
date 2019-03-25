using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents
{
    public class FatSearchViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string searchRouteName, string keywords)
        {
            var model = new FatSearchViewModel
            {
                Keywords = keywords,
                SearchRouteName = searchRouteName
            };

            return View(model);
        }
    }
}
