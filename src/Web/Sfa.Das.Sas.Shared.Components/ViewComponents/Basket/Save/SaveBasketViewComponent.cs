using Microsoft.AspNetCore.Mvc;
using Sfa.Das.Sas.Core.Configuration;
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
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            var model = new SaveBasketViewModel
            {
                BasketId = cookieBasketId,
                SaveBasketUrl = _config.SaveEmployerFavouritesUrl
            };

            return View("../Basket/Save/Default", model);
        }
    }
}
