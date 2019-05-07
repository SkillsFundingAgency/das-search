using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Core.Configuration;
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
            var saveUrl = new Uri(new Uri(_config.EmployerFavouritesUrl), "/save-apprenticeship-favourites");
            var builder = new UriBuilder(saveUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);

            query["apprenticeshipId"] = apprenticeshipId;

            builder.Query = query.ToString();

            var model = new SaveApprenticeshipViewModel
            {
                SaveUrl = builder.Uri
            };

            return View("Default", model);
        }
    }
}
