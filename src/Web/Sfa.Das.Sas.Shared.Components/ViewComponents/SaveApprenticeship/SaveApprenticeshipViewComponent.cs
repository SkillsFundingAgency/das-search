using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Configuration;
using Sfa.Das.Sas.Shared.Components.ViewComponents.SaveApprenticeship;
using System;
using System.Web;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Basket.SaveApprenticeship
{
    public class SaveApprenticeshipViewComponent : ViewComponent
    {
        private readonly IFatConfigurationSettings _config;

        public SaveApprenticeshipViewComponent(IFatConfigurationSettings config)
        {
            _config = config;
        }

        public IViewComponentResult Invoke(string apprenticeshipId)
        {
            var uriBuilder = new SaveApprenticeshipUrlBuilder(_config);

            var model = new SaveApprenticeshipViewModel
            {
                SaveUrl = uriBuilder.GenerateSaveUrl(apprenticeshipId)
            };

            return View("Default", model);
        }
    }
}
