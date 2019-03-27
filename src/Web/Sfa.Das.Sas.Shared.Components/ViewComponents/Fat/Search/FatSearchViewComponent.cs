using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents
{
    public class FatSearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string searchRouteName, string keywords)
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
