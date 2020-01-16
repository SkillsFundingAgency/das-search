using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Core.Configuration;
using Sfa.Das.Sas.Shared.Components.Configuration;
using Sfa.Das.Sas.Shared.Components.Cookies;
using System;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Basket
{
    public class SaveBasketViewComponent : ViewComponent
    {
        private readonly IFatConfigurationSettings _config;
        private readonly ICookieManager _cookieManager;

        public SaveBasketViewComponent(IFatConfigurationSettings config, ICookieManager cookieManager)
        {
            _config = config;
            _cookieManager = cookieManager;
        }

        public IViewComponentResult Invoke()
        {
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            var basketIdForQueryString = Guid.TryParse(cookie, out Guid result) ? result.ToString() : string.Empty;

            var uriBuilder = new SaveApprenticeshipUrlBuilder(_config);

            var model = new SaveBasketViewModel
            {
                SaveBasketUrl = uriBuilder.GenerateSaveUrl(basketIdForQueryString).ToString(),
                BasketId = basketIdForQueryString
            };

            return View("../Basket/Save/Default", model);
        }
    }
}
