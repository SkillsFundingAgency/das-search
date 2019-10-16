using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Sfa.Das.Sas.ApplicationServices.Queries;
using Sfa.Das.Sas.Shared.Components.Cookies;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Das.Sas.Shared.Components.ViewComponents.Basket
{
    public class BasketIconViewComponent : ViewComponent
    {
        private readonly IMediator _mediator;
        private readonly ICookieManager _cookieManager;

        public BasketIconViewComponent(IMediator mediator, ICookieManager cookieManager)
        {
            _mediator = mediator;
            _cookieManager = cookieManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new BasketIconViewModel
            {
                ItemCount = await GetBasketItemCount(),
                BasketUrl = Url.Link("BasketView", null)
            };

            return View("../Basket/BasketIcon/Default", model);
        }

        private async Task<int> GetBasketItemCount()
        {
            // Get cookie
            var cookie = _cookieManager.Get(CookieNames.BasketCookie);
            Guid? cookieBasketId = Guid.TryParse(cookie, out Guid result) ? (Guid?)result : null;

            if (cookieBasketId.HasValue)
            {
                var basket = await _mediator.Send(new GetBasketQuery { BasketId = cookieBasketId.Value });

                return basket?.Count() ?? 0;
            }

            return 0;
        }
    }
}
