using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents
{
    public class FatSearchViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string searchRouteName, string keywords, string classPrefix = "govuk-", string classModifier = null, bool inline = false)
        {
            var model = new FatSearchViewModel
            {
                Keywords = keywords,
                SearchRouteName = searchRouteName,
                ClassPrefix = classPrefix,
                ClassModifier = classModifier
            };

            if (!inline)
            {
                return View(model);
            }
            else{
                return View("Inline", model);
            }

        }
    }
}
