using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Shared.Components.Domain.Interfaces;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Fat
{
    public class FatSearchViewComponent : ViewComponent
    {
        private readonly ICssClasses _cssClasses;

        public FatSearchViewComponent(ICssClasses cssClasses)
        {
            _cssClasses = cssClasses;
        }

        public async Task<IViewComponentResult> InvokeAsync(string searchRouteName, string keywords,string cssModifier = null, bool inline = false)
        {
            if (cssModifier != null)
            {
                _cssClasses.ClassModifier = cssModifier;
            }

            var model = new FatSearchViewModel
            {
                Keywords = keywords,
                SearchRouteName = searchRouteName,
                CssClasses = _cssClasses
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
